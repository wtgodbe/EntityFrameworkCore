// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Query
{
    /// <summary>
    ///     <para>
    ///         Service dependencies parameter class for <see cref="QueryCompilationContext" />
    ///     </para>
    ///     <para>
    ///         This type is typically used by database providers (and other extensions). It is generally
    ///         not used in application code.
    ///     </para>
    ///     <para>
    ///         Do not construct instances of this class directly from either provider or application code as the
    ///         constructor signature may change as new dependencies are added. Instead, use this type in
    ///         your constructor so that an instance will be created and injected automatically by the
    ///         dependency injection container. To create an instance with some dependent services replaced,
    ///         first resolve the object from the dependency injection container, then replace selected
    ///         services using the 'With...' methods. Do not call the constructor at any point in this process.
    ///     </para>
    ///     <para>
    ///         The service lifetime is <see cref="ServiceLifetime.Scoped" />. This means that each
    ///         <see cref="DbContext" /> instance will use its own instance of this service.
    ///         The implementation may depend on other services registered with any lifetime.
    ///         The implementation does not need to be thread-safe.
    ///     </para>
    /// </summary>
    public sealed class QueryCompilationContextDependencies
    {
        /// <summary>
        ///     <para>
        ///         Creates the service dependencies parameter object for a <see cref="QueryCompilationContext" />.
        ///     </para>
        ///     <para>
        ///         Do not call this constructor directly from either provider or application code as it may change
        ///         as new dependencies are added. Instead, use this type in your constructor so that an instance
        ///         will be created and injected automatically by the dependency injection container. To create
        ///         an instance with some dependent services replaced, first resolve the object from the dependency
        ///         injection container, then replace selected services using the 'With...' methods. Do not call
        ///         the constructor at any point in this process.
        ///     </para>
        ///     <para>
        ///         This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///         the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///         any release. You should only use it directly in your code with extreme caution and knowing that
        ///         doing so can result in application failures when updating to a new Entity Framework Core release.
        ///     </para>
        /// </summary>
        [EntityFrameworkInternal]
        public QueryCompilationContextDependencies(
            [NotNull] IModel model,
            [NotNull] IQueryOptimizerFactory queryOptimizerFactory,
            [NotNull] IQueryableMethodTranslatingExpressionVisitorFactory queryableMethodTranslatingExpressionVisitorFactory,
            [NotNull] IShapedQueryOptimizerFactory shapedQueryOptimizerFactory,
            [NotNull] IShapedQueryCompilingExpressionVisitorFactory shapedQueryCompilingExpressionVisitorFactory,
            [NotNull] ICurrentDbContext currentContext,
            [NotNull] IDbContextOptions contextOptions,
            [NotNull] IDiagnosticsLogger<DbLoggerCategory.Query> logger,
            [NotNull] IEvaluatableExpressionFilter evaluatableExpressionFilter)
        {
            Check.NotNull(model, nameof(model));
            Check.NotNull(queryOptimizerFactory, nameof(queryOptimizerFactory));
            Check.NotNull(queryableMethodTranslatingExpressionVisitorFactory, nameof(queryableMethodTranslatingExpressionVisitorFactory));
            Check.NotNull(shapedQueryOptimizerFactory, nameof(shapedQueryOptimizerFactory));
            Check.NotNull(shapedQueryCompilingExpressionVisitorFactory, nameof(shapedQueryCompilingExpressionVisitorFactory));
            Check.NotNull(currentContext, nameof(currentContext));
            Check.NotNull(contextOptions, nameof(contextOptions));
            Check.NotNull(logger, nameof(logger));
            Check.NotNull(evaluatableExpressionFilter, nameof(evaluatableExpressionFilter));

            CurrentContext = currentContext;
            Model = model;
            QueryOptimizerFactory = queryOptimizerFactory;
            QueryableMethodTranslatingExpressionVisitorFactory = queryableMethodTranslatingExpressionVisitorFactory;
            ShapedQueryOptimizerFactory = shapedQueryOptimizerFactory;
            ShapedQueryCompilingExpressionVisitorFactory = shapedQueryCompilingExpressionVisitorFactory;
            ContextOptions = contextOptions;
            Logger = logger;
            EvaluatableExpressionFilter = evaluatableExpressionFilter;
        }

        /// <summary>
        ///     The cache being used to store value generator instances.
        /// </summary>
        public ICurrentDbContext CurrentContext { get; }

        /// <summary>
        ///     The model.
        /// </summary>
        public IModel Model { get; }

        /// <summary>
        ///     The query optimizer factory.
        /// </summary>
        public IQueryOptimizerFactory QueryOptimizerFactory { get; }

        /// <summary>
        ///     The queryable method-translating expression visitor factory.
        /// </summary>
        public IQueryableMethodTranslatingExpressionVisitorFactory QueryableMethodTranslatingExpressionVisitorFactory { get; }

        /// <summary>
        ///     The shaped-query optimizer factory
        /// </summary>
        public IShapedQueryOptimizerFactory ShapedQueryOptimizerFactory { get; }

        /// <summary>
        ///     The shaped-query compiling expression visitor factory.
        /// </summary>
        public IShapedQueryCompilingExpressionVisitorFactory ShapedQueryCompilingExpressionVisitorFactory { get; }

        /// <summary>
        ///     The context options.
        /// </summary>
        public IDbContextOptions ContextOptions { get; }

        /// <summary>
        ///     The logger.
        /// </summary>
        public IDiagnosticsLogger<DbLoggerCategory.Query> Logger { get; }

        /// <summary>
        ///     Evaluatable expression filter.
        /// </summary>
        public IEvaluatableExpressionFilter EvaluatableExpressionFilter { get; }

        /// <summary>
        ///     Clones this dependency parameter object with one service replaced.
        /// </summary>
        /// <param name="model"> A replacement for the current dependency of this type. </param>
        /// <returns> A new parameter object with the given service replaced. </returns>
        public QueryCompilationContextDependencies With([NotNull] IModel model)
            => new QueryCompilationContextDependencies(
                model,
                QueryOptimizerFactory,
                QueryableMethodTranslatingExpressionVisitorFactory,
                ShapedQueryOptimizerFactory,
                ShapedQueryCompilingExpressionVisitorFactory,
                CurrentContext,
                ContextOptions,
                Logger,
                EvaluatableExpressionFilter);

        /// <summary>
        ///     Clones this dependency parameter object with one service replaced.
        /// </summary>
        /// <param name="queryOptimizerFactory"> A replacement for the current dependency of this type. </param>
        /// <returns> A new parameter object with the given service replaced. </returns>
        public QueryCompilationContextDependencies With([NotNull] IQueryOptimizerFactory queryOptimizerFactory)
            => new QueryCompilationContextDependencies(
                Model,
                queryOptimizerFactory,
                QueryableMethodTranslatingExpressionVisitorFactory,
                ShapedQueryOptimizerFactory,
                ShapedQueryCompilingExpressionVisitorFactory,
                CurrentContext,
                ContextOptions,
                Logger,
                EvaluatableExpressionFilter);

        /// <summary>
        ///     Clones this dependency parameter object with one service replaced.
        /// </summary>
        /// <param name="queryableMethodTranslatingExpressionVisitorFactory"> A replacement for the current dependency of this type. </param>
        /// <returns> A new parameter object with the given service replaced. </returns>
        public QueryCompilationContextDependencies With(
            [NotNull] IQueryableMethodTranslatingExpressionVisitorFactory queryableMethodTranslatingExpressionVisitorFactory)
            => new QueryCompilationContextDependencies(
                Model,
                QueryOptimizerFactory,
                queryableMethodTranslatingExpressionVisitorFactory,
                ShapedQueryOptimizerFactory,
                ShapedQueryCompilingExpressionVisitorFactory,
                CurrentContext,
                ContextOptions,
                Logger,
                EvaluatableExpressionFilter);

        /// <summary>
        ///     Clones this dependency parameter object with one service replaced.
        /// </summary>
        /// <param name="shapedQueryOptimizerFactory"> A replacement for the current dependency of this type. </param>
        /// <returns> A new parameter object with the given service replaced. </returns>
        public QueryCompilationContextDependencies With([NotNull] IShapedQueryOptimizerFactory shapedQueryOptimizerFactory)
            => new QueryCompilationContextDependencies(
                Model,
                QueryOptimizerFactory,
                QueryableMethodTranslatingExpressionVisitorFactory,
                shapedQueryOptimizerFactory,
                ShapedQueryCompilingExpressionVisitorFactory,
                CurrentContext,
                ContextOptions,
                Logger,
                EvaluatableExpressionFilter);

        /// <summary>
        ///     Clones this dependency parameter object with one service replaced.
        /// </summary>
        /// <param name="shapedQueryCompilingExpressionVisitorFactory"> A replacement for the current dependency of this type. </param>
        /// <returns> A new parameter object with the given service replaced. </returns>
        public QueryCompilationContextDependencies With(
            [NotNull] IShapedQueryCompilingExpressionVisitorFactory shapedQueryCompilingExpressionVisitorFactory)
            => new QueryCompilationContextDependencies(
                Model,
                QueryOptimizerFactory,
                QueryableMethodTranslatingExpressionVisitorFactory,
                ShapedQueryOptimizerFactory,
                shapedQueryCompilingExpressionVisitorFactory,
                CurrentContext,
                ContextOptions,
                Logger,
                EvaluatableExpressionFilter);

        /// <summary>
        ///     Clones this dependency parameter object with one service replaced.
        /// </summary>
        /// <param name="currentContext"> A replacement for the current dependency of this type. </param>
        /// <returns> A new parameter object with the given service replaced. </returns>
        public QueryCompilationContextDependencies With([NotNull] ICurrentDbContext currentContext)
            => new QueryCompilationContextDependencies(
                Model,
                QueryOptimizerFactory,
                QueryableMethodTranslatingExpressionVisitorFactory,
                ShapedQueryOptimizerFactory,
                ShapedQueryCompilingExpressionVisitorFactory,
                currentContext,
                ContextOptions,
                Logger,
                EvaluatableExpressionFilter);

        /// <summary>
        ///     Clones this dependency parameter object with one service replaced.
        /// </summary>
        /// <param name="contextOptions"> A replacement for the current dependency of this type. </param>
        /// <returns> A new parameter object with the given service replaced. </returns>
        public QueryCompilationContextDependencies With([NotNull] IDbContextOptions contextOptions)
            => new QueryCompilationContextDependencies(
                Model,
                QueryOptimizerFactory,
                QueryableMethodTranslatingExpressionVisitorFactory,
                ShapedQueryOptimizerFactory,
                ShapedQueryCompilingExpressionVisitorFactory,
                CurrentContext,
                contextOptions,
                Logger,
                EvaluatableExpressionFilter);

        /// <summary>
        ///     Clones this dependency parameter object with one service replaced.
        /// </summary>
        /// <param name="logger"> A replacement for the current dependency of this type. </param>
        /// <returns> A new parameter object with the given service replaced. </returns>
        public QueryCompilationContextDependencies With([NotNull] IDiagnosticsLogger<DbLoggerCategory.Query> logger)
            => new QueryCompilationContextDependencies(
                Model,
                QueryOptimizerFactory,
                QueryableMethodTranslatingExpressionVisitorFactory,
                ShapedQueryOptimizerFactory,
                ShapedQueryCompilingExpressionVisitorFactory,
                CurrentContext,
                ContextOptions,
                logger,
                EvaluatableExpressionFilter);

        /// <summary>
        ///     Clones this dependency parameter object with one service replaced.
        /// </summary>
        /// <param name="evaluatableExpressionFilter"> A replacement for the current dependency of this type. </param>
        /// <returns> A new parameter object with the given service replaced. </returns>
        public QueryCompilationContextDependencies With([NotNull] IEvaluatableExpressionFilter evaluatableExpressionFilter)
            => new QueryCompilationContextDependencies(
                Model,
                QueryOptimizerFactory,
                QueryableMethodTranslatingExpressionVisitorFactory,
                ShapedQueryOptimizerFactory,
                ShapedQueryCompilingExpressionVisitorFactory,
                CurrentContext,
                ContextOptions,
                Logger,
                evaluatableExpressionFilter);
    }
}
