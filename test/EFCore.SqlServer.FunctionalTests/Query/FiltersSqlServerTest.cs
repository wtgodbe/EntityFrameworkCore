﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming
namespace Microsoft.EntityFrameworkCore.Query
{
    public class FiltersSqlServerTest : FiltersTestBase<NorthwindQuerySqlServerFixture<NorthwindFiltersCustomizer>>
    {
        public FiltersSqlServerTest(NorthwindQuerySqlServerFixture<NorthwindFiltersCustomizer> fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            fixture.TestSqlLoggerFactory.Clear();
            //fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        public override void Count_query()
        {
            base.Count_query();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='B' (Size = 4000)

SELECT COUNT(*)
FROM [Customers] AS [c]
WHERE ((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))");
        }

        public override void Client_eval()
        {
            base.Client_eval();

            AssertSql(
                @"SELECT [p].[ProductID], [p].[Discontinued], [p].[ProductName], [p].[SupplierID], [p].[UnitPrice], [p].[UnitsInStock]
FROM [Products] AS [p]");
        }

        public override void Materialized_query()
        {
            base.Materialized_query();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='B' (Size = 4000)

SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
WHERE ((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))");
        }

        public override void Find()
        {
            base.Find();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='B' (Size = 4000)
@__p_0='ALFKI' (Size = 5)

SELECT TOP(1) [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
WHERE (((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))) AND (([c].[CustomerID] = @__p_0) AND @__p_0 IS NOT NULL)");
        }

        public override void Materialized_query_parameter()
        {
            base.Materialized_query_parameter();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='F' (Size = 4000)

SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
WHERE ((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))");
        }

        public override void Materialized_query_parameter_new_context()
        {
            base.Materialized_query_parameter_new_context();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='B' (Size = 4000)

SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
WHERE ((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))",
                //
                @"@__ef_filter__TenantPrefix_0='T' (Size = 4000)

SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
WHERE ((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))");
        }

        public override void Projection_query_parameter()
        {
            base.Projection_query_parameter();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='F' (Size = 4000)

SELECT [c].[CustomerID]
FROM [Customers] AS [c]
WHERE ((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))");
        }

        public override void Projection_query()
        {
            base.Projection_query();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='B' (Size = 4000)

SELECT [c].[CustomerID]
FROM [Customers] AS [c]
WHERE ((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))");
        }

        public override void Include_query()
        {
            base.Include_query();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='B' (Size = 4000)

SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region], [t].[OrderID], [t].[CustomerID], [t].[EmployeeID], [t].[OrderDate]
FROM [Customers] AS [c]
LEFT JOIN (
    SELECT [o].[OrderID], [o].[CustomerID], [o].[EmployeeID], [o].[OrderDate]
    FROM [Orders] AS [o]
    LEFT JOIN [Customers] AS [c0] ON [o].[CustomerID] = [c0].[CustomerID]
    WHERE [c0].[CompanyName] IS NOT NULL
) AS [t] ON [c].[CustomerID] = [t].[CustomerID]
WHERE ((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))
ORDER BY [c].[CustomerID], [t].[OrderID]");
        }

        public override void Include_query_opt_out()
        {
            base.Include_query_opt_out();

            AssertSql(
                @"SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region], [o].[OrderID], [o].[CustomerID], [o].[EmployeeID], [o].[OrderDate]
FROM [Customers] AS [c]
LEFT JOIN [Orders] AS [o] ON [c].[CustomerID] = [o].[CustomerID]
ORDER BY [c].[CustomerID], [o].[OrderID]");
        }

        public override void Included_many_to_one_query()
        {
            base.Included_many_to_one_query();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='B' (Size = 4000)

SELECT [o].[OrderID], [o].[CustomerID], [o].[EmployeeID], [o].[OrderDate], [t].[CustomerID], [t].[Address], [t].[City], [t].[CompanyName], [t].[ContactName], [t].[ContactTitle], [t].[Country], [t].[Fax], [t].[Phone], [t].[PostalCode], [t].[Region]
FROM [Orders] AS [o]
LEFT JOIN (
    SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
    FROM [Customers] AS [c]
    WHERE ((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))
) AS [t] ON [o].[CustomerID] = [t].[CustomerID]
WHERE [t].[CompanyName] IS NOT NULL");
        }

        public override void Project_reference_that_itself_has_query_filter_with_another_reference()
        {
            base.Project_reference_that_itself_has_query_filter_with_another_reference();

            AssertSql(
                @"@__ef_filter__TenantPrefix_1='B' (Size = 4000)
@__ef_filter___quantity_0='50'

SELECT [t0].[OrderID], [t0].[CustomerID], [t0].[EmployeeID], [t0].[OrderDate]
FROM [Order Details] AS [o0]
INNER JOIN (
    SELECT [o].[OrderID], [o].[CustomerID], [o].[EmployeeID], [o].[OrderDate], [t].[CustomerID] AS [CustomerID0], [t].[Address], [t].[City], [t].[CompanyName], [t].[ContactName], [t].[ContactTitle], [t].[Country], [t].[Fax], [t].[Phone], [t].[PostalCode], [t].[Region]
    FROM [Orders] AS [o]
    LEFT JOIN (
        SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
        FROM [Customers] AS [c]
        WHERE ((@__ef_filter__TenantPrefix_1 = N'') AND @__ef_filter__TenantPrefix_1 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_1 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_1)) = @__ef_filter__TenantPrefix_1) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_1)) IS NOT NULL AND @__ef_filter__TenantPrefix_1 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_1)) IS NULL AND @__ef_filter__TenantPrefix_1 IS NULL)))))
    ) AS [t] ON [o].[CustomerID] = [t].[CustomerID]
    WHERE [t].[CompanyName] IS NOT NULL
) AS [t0] ON [o0].[OrderID] = [t0].[OrderID]
WHERE [o0].[Quantity] > @__ef_filter___quantity_0");
        }

        public override void Included_one_to_many_query_with_client_eval()
        {
            base.Included_one_to_many_query_with_client_eval();

            AssertSql(
                @"SELECT [p].[ProductID], [p].[Discontinued], [p].[ProductName], [p].[SupplierID], [p].[UnitPrice], [p].[UnitsInStock]
FROM [Products] AS [p]
ORDER BY [p].[ProductID]",
                //
                @"SELECT [p1].[ProductID], [p1].[Discontinued], [p1].[ProductName], [p1].[SupplierID], [p1].[UnitPrice], [p1].[UnitsInStock]
FROM [Products] AS [p1]",
                //
                @"@__ef_filter___quantity_0='50' (DbType = Int16)

SELECT [od].[OrderID], [od].[ProductID], [od].[Discount], [od].[Quantity], [od].[UnitPrice]
FROM [Order Details] AS [od]
WHERE [od].[Quantity] > @__ef_filter___quantity_0");
        }

        public override void Navs_query()
        {
            base.Navs_query();

            AssertSql(
                @"@__ef_filter___quantity_1='50' (DbType = Int16)
@__ef_filter__TenantPrefix_0='B' (Size = 4000)

SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
INNER JOIN (
    SELECT [o].*
    FROM [Orders] AS [o]
    LEFT JOIN [Customers] AS [o.Customer] ON [o].[CustomerID] = [o.Customer].[CustomerID]
    WHERE [o.Customer].[CompanyName] IS NOT NULL
) AS [t] ON [c].[CustomerID] = [t].[CustomerID]
INNER JOIN (
    SELECT [od].*
    FROM [Order Details] AS [od]
    WHERE [od].[Quantity] > @__ef_filter___quantity_1
) AS [t0] ON [t].[OrderID] = [t0].[OrderID]
WHERE (([c].[CompanyName] LIKE @__ef_filter__TenantPrefix_0 + N'%' AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0)) OR (@__ef_filter__TenantPrefix_0 = N'')) AND ([t0].[Discount] < CAST(10 AS real))");
        }

        [ConditionalFact(Skip = "issue #16326")]
        public void FromSql_is_composed()
        {
            using (var context = CreateContext())
            {
                var results = context.Customers.FromSqlRaw("select * from Customers").ToList();

                Assert.Equal(7, results.Count);
            }

            AssertSql(
                @"");
        }


        [ConditionalFact(Skip = "issue #16326")]
        public void FromSql_is_composed_when_filter_has_navigation()
        {
            using (var context = CreateContext())
            {
                var results = context.Orders.FromSqlRaw("select * from Orders").ToList();

                Assert.Equal(9999, results.Count);
            }

            AssertSql(
                @"");
        }

        public override void Compiled_query()
        {
            base.Compiled_query();

            AssertSql(
                @"@__ef_filter__TenantPrefix_0='B' (Size = 4000)
@__customerID='BERGS' (Size = 5)

SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
WHERE (((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))) AND (([c].[CustomerID] = @__customerID) AND @__customerID IS NOT NULL)",
                //
                @"@__ef_filter__TenantPrefix_0='B' (Size = 4000)
@__customerID='BLAUS' (Size = 5)

SELECT [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
WHERE (((@__ef_filter__TenantPrefix_0 = N'') AND @__ef_filter__TenantPrefix_0 IS NOT NULL) OR ([c].[CompanyName] IS NOT NULL AND (@__ef_filter__TenantPrefix_0 IS NOT NULL AND (([c].[CompanyName] LIKE [c].[CompanyName] + N'%') AND (((LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) = @__ef_filter__TenantPrefix_0) AND (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NOT NULL AND @__ef_filter__TenantPrefix_0 IS NOT NULL)) OR (LEFT([c].[CompanyName], LEN(@__ef_filter__TenantPrefix_0)) IS NULL AND @__ef_filter__TenantPrefix_0 IS NULL)))))) AND (([c].[CustomerID] = @__customerID) AND @__customerID IS NOT NULL)");
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);
    }
}
