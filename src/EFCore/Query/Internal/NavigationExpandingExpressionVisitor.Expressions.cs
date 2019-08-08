﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Query.Internal
{
    public partial class NavigationExpandingExpressionVisitor
    {
        public class NavigationExpansionExpression : Expression, IPrintable
        {
            private readonly List<(MethodInfo OrderingMethod, Expression KeySelector)> _pendingOrderings
                = new List<(MethodInfo OrderingMethod, Expression KeySelector)>();
            private readonly string _parameterName;
            private NavigationTreeNode _currentTree;

            public NavigationExpansionExpression(
                Expression source, NavigationTreeNode currentTree, Expression pendingSelector, string parameterName)
            {
                Source = source;
                _parameterName = parameterName;
                CurrentTree = currentTree;
                PendingSelector = pendingSelector;
            }

            public virtual Expression Source { get; private set; }
            public virtual ParameterExpression CurrentParameter => CurrentTree.CurrentParameter;
            public virtual NavigationTreeNode CurrentTree
            {
                get => _currentTree;
                private set
                {
                    _currentTree = value;
                    _currentTree.SetParameter(_parameterName);
                }
            }
            public virtual Expression PendingSelector { get; private set; }
            public virtual MethodInfo CardinalityReducingGenericMethodInfo { get; private set; }
            public virtual Type SourceElementType => CurrentParameter.Type;
            public virtual IReadOnlyList<(MethodInfo OrderingMethod, Expression KeySelector)> PendingOrderings => _pendingOrderings;

            public virtual void UpdateSource(Expression source)
            {
                Source = source;
            }

            public virtual void UpdateCurrentTree(NavigationTreeNode currentTree)
            {
                CurrentTree = currentTree;
            }

            public virtual void ApplySelector(Expression selector)
            {
                PendingSelector = selector;
            }

            public virtual void AddPendingOrdering(MethodInfo orderingMethod, Expression keySelector)
            {
                _pendingOrderings.Clear();
                _pendingOrderings.Add((orderingMethod, keySelector));
            }
            public virtual void AppendPendingOrdering(MethodInfo orderingMethod, Expression keySelector)
            {
                _pendingOrderings.Add((orderingMethod, keySelector));
            }

            public virtual void ClearPendingOrderings()
            {
                _pendingOrderings.Clear();
            }

            public virtual void ConvertToSingleResult(MethodInfo genericMethod)
            {
                CardinalityReducingGenericMethodInfo = genericMethod;
            }

            protected override Expression VisitChildren(ExpressionVisitor visitor) => this;

            public virtual void Print(ExpressionPrinter expressionPrinter)
            {
                expressionPrinter.Visit(Source);
                expressionPrinter.StringBuilder.Append(".PendingSelector(");
                expressionPrinter.Visit(PendingSelector);
                expressionPrinter.StringBuilder.Append(")");

                if (CardinalityReducingGenericMethodInfo != null)
                {
                    expressionPrinter.StringBuilder.Append("." + CardinalityReducingGenericMethodInfo.Name + "()");
                }
            }

            public override ExpressionType NodeType => ExpressionType.Extension;
            public override Type Type => CardinalityReducingGenericMethodInfo == null
                ? typeof(IQueryable<>).MakeGenericType(PendingSelector.Type)
                : PendingSelector.Type;
        }

        public class NavigationTreeExpression : NavigationTreeNode, IPrintable
        {
            public NavigationTreeExpression(Expression value)
                : base(null, null)
            {
                Value = value;
            }
            public virtual Expression Value { get; private set; }
            protected override Expression VisitChildren(ExpressionVisitor visitor)
            {
                Value = visitor.Visit(Value);

                return this;
            }
            public override Type Type => Value.Type;

            public override void Print(ExpressionPrinter expressionPrinter)
            {
                expressionPrinter.Append("NAVIGATION_TREE_EXPRESSION(");

                var parent = Parent;
                var currentParameter = CurrentParameter;
                while (currentParameter == null)
                {
                    currentParameter = parent.CurrentParameter;
                    parent = parent.Parent;
                }

                expressionPrinter.Visit(currentParameter);
                expressionPrinter.Append(" | ");
                expressionPrinter.Visit(Value);
                expressionPrinter.Append(")");
            }
        }

        public class EntityReference : Expression, IPrintable
        {
            public EntityReference(IEntityType entityType)
            {
                EntityType = entityType;
                IncludePaths = new IncludeTreeNode(entityType, this);
            }

            public virtual IEntityType EntityType { get; }
            public virtual IDictionary<INavigation, Expression> NavigationMap { get; }
                = new Dictionary<INavigation, Expression>();

            public virtual IncludeTreeNode IncludePaths { get; private set; }
            public virtual IncludeTreeNode LastIncludeTreeNode { get; private set; }

            protected override Expression VisitChildren(ExpressionVisitor visitor) => this;

            public virtual void SetIncludePaths(IncludeTreeNode includePaths)
            {
                IncludePaths = includePaths;
                includePaths.SetEntityReference(this);
            }

            public virtual EntityReference Clone()
            {
                var result = new EntityReference(EntityType)
                {
                    IsOptional = IsOptional
                };
                result.IncludePaths = IncludePaths.Clone(result);

                return result;
            }

            public virtual void SetLastInclude(IncludeTreeNode lastIncludeTree)
            {
                LastIncludeTreeNode = lastIncludeTree;
            }

            public virtual void MarkAsOptional()
            {
                IsOptional = true;
            }

            public virtual void Print(ExpressionPrinter expressionPrinter)
            {
                expressionPrinter.Append("ENTITY_REFERENCE(");
                expressionPrinter.Append(EntityType.DisplayName());

                if (IncludePaths.Count > 0)
                {
                    expressionPrinter.Append(" | IncludePaths: " + string.Join(".", IncludePaths.Select(ip => ip.Key.Name)));
                }

                expressionPrinter.Append(")");
            }

            public virtual bool IsOptional { get; private set; }

            public override ExpressionType NodeType => ExpressionType.Extension;
            public override Type Type => EntityType.ClrType;
        }

        public class NavigationTreeNode : Expression, IPrintable
        {
            private NavigationTreeNode _parent;

            public NavigationTreeNode(NavigationTreeNode left, NavigationTreeNode right)
            {
                Left = left;
                Right = right;
                if (left != null)
                {
                    Left.Parent = this;
                    Right.Parent = this;
                }
            }

            public virtual NavigationTreeNode Parent
            {
                get => _parent;
                private set
                {
                    _parent = value;
                    CurrentParameter = null;
                }
            }
            public virtual NavigationTreeNode Left { get; }
            public virtual NavigationTreeNode Right { get; }
            public virtual ParameterExpression CurrentParameter { get; private set; }

            protected override Expression VisitChildren(ExpressionVisitor visitor) => throw new InvalidOperationException();

            public virtual void SetParameter(string parameterName)
            {
                CurrentParameter = Parameter(Type, parameterName);
            }

            public override ExpressionType NodeType => ExpressionType.Extension;
            public override Type Type => TransparentIdentifierFactory.Create(Left.Type, Right.Type);
            public virtual Expression GetExpression()
            {
                if (Parent == null)
                {
                    return CurrentParameter;
                }

                var parentExperssion = Parent.GetExpression();
                return Parent.Left == this
                    ? MakeMemberAccess(parentExperssion, parentExperssion.Type.GetTypeInfo().GetMember("Outer")[0])
                    : MakeMemberAccess(parentExperssion, parentExperssion.Type.GetTypeInfo().GetMember("Inner")[0]);
            }

            public virtual void Print(ExpressionPrinter expressionPrinter)
            {
                expressionPrinter.Append("NAVIGATION_TREE_NODE(");
                expressionPrinter.Visit(CurrentParameter);
                expressionPrinter.Append(")");
            }
        }

        public class OwnedNavigationReference : Expression
        {
            public OwnedNavigationReference(Expression parent, INavigation navigation, EntityReference entityReference)
            {
                Parent = parent;
                Navigation = navigation;
                EntityReference = entityReference;
            }

            protected override Expression VisitChildren(ExpressionVisitor visitor)
            {
                Parent = visitor.Visit(Parent);

                return this;
            }

            public override Type Type => Navigation.ClrType;
            public override ExpressionType NodeType => ExpressionType.Extension;

            public virtual Expression Parent { get; private set; }
            public virtual INavigation Navigation { get; }
            public virtual EntityReference EntityReference { get; }
        }

        public class IncludeTreeNode : Dictionary<INavigation, IncludeTreeNode>
        {
            private EntityReference _entityReference;
            public virtual IEntityType EntityType { get; private set; }

            public IncludeTreeNode(IEntityType entityType, EntityReference entityReference)
            {
                EntityType = entityType;
                _entityReference = entityReference;
            }

            public virtual IncludeTreeNode AddNavigation(INavigation navigation)
            {
                if (TryGetValue(navigation, out var existingValue))
                {
                    return existingValue;
                }

                if (_entityReference != null
                    && _entityReference.NavigationMap.TryGetValue(navigation, out var expandedNavigation))
                {
                    var entityReference = expandedNavigation switch
                    {
                        NavigationTreeExpression navigationTree => (EntityReference)navigationTree.Value,
                        OwnedNavigationReference ownedNavigationReference => ownedNavigationReference.EntityReference,
                        _ => throw new InvalidOperationException("Invalid expression type stored in NavigationMap."),
                    };

                    this[navigation] = entityReference.IncludePaths;
                }
                else
                {
                    this[navigation] = new IncludeTreeNode(navigation.GetTargetType(), null);
                }

                return this[navigation];
            }

            public virtual IncludeTreeNode Clone(EntityReference entityReference)
            {
                var result = new IncludeTreeNode(EntityType, entityReference);
                foreach (var kvp in this)
                {
                    result[kvp.Key] = kvp.Value.Clone(kvp.Value._entityReference);
                }

                return result;
            }

            public override bool Equals(object obj)
                => obj != null
                && (ReferenceEquals(this, obj)
                    || obj is IncludeTreeNode includeTreeNode
                        && Equals(includeTreeNode));

            private bool Equals(IncludeTreeNode includeTreeNode)
            {
                if (Count != includeTreeNode.Count)
                {
                    return false;
                }

                foreach (var kvp in this)
                {
                    if (!includeTreeNode.TryGetValue(kvp.Key, out var otherIncludeTreeNode)
                        || !kvp.Value.Equals(otherIncludeTreeNode))
                    {
                        return false;
                    }
                }

                return true;
            }

            public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), EntityType);

            public virtual void SetEntityReference(EntityReference entityReference)
            {
                _entityReference = entityReference;
                EntityType = entityReference.EntityType;
            }
        }
    }
}
