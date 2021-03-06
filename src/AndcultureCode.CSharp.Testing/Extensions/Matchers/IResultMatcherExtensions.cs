﻿using System.Linq;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Testing.Constants;
using Shouldly;
using CoreErrorConstants = AndcultureCode.CSharp.Core.Constants.ErrorConstants;

namespace AndcultureCode.CSharp.Testing.Extensions
{
    /// <summary>
    /// Extension methods for asserting expected states of the `IResult` interface
    /// </summary>
    public static class IResultMatcherExtensions
    {
        #region Constants

        /// <summary>
        /// Detailed output message to display when expecting errors on a result that has a null `Errors` property
        /// </summary>
        public const string ERROR_ERRORS_LIST_IS_NULL_MESSAGE = "Expected result to have errors, but instead Errors is 'null'";

        #endregion Constants

        #region Public Methods

        /// <summary>
        /// Assert result has error for `BASIC_ERROR_KEY`
        /// </summary>
        /// <param name="result">Result under test</param>
        /// <typeparam name="T"></typeparam>
        public static void ShouldHaveBasicError<T>(this IResult<T> result) => result.ShouldHaveErrorsFor(ErrorConstants.BASIC_ERROR_KEY);

        /// <summary>
        /// Assert that the result has at least one error
        /// </summary>
        /// <param name="result">Result under test</param>
        /// <param name="exactCount">When supplied, asserts the result has this exact number of errors</param>
        /// <typeparam name="T"></typeparam>
        public static void ShouldHaveErrors<T>(this IResult<T> result, int? exactCount = null)
        {
            result.ShouldNotBeNull();
            result.Errors.ShouldNotBeNull(ERROR_ERRORS_LIST_IS_NULL_MESSAGE);
            result.Errors.Count.ShouldBeGreaterThan(0);
            result.ErrorCount.ShouldBeGreaterThan(0);
            result.HasErrors.ShouldBeTrue(result.ListErrors());

            if (exactCount != null)
            {
                result.ErrorCount.ShouldBe((int)exactCount);
                result.Errors.Count.ShouldBe((int)exactCount);
            }
        }

        /// <summary>
        /// Assert that the result has at least one error
        /// </summary>
        /// <param name="result">Result under test</param>
        /// <param name="exactCount">When supplied, asserts the result has this exact number of errors</param>
        public static void ShouldHaveErrors(this IResult<bool> result, int? exactCount = null)
        {
            result.ShouldNotBeNull();
            result.Errors.ShouldNotBeNull(ERROR_ERRORS_LIST_IS_NULL_MESSAGE);
            result.Errors.Count.ShouldBeGreaterThan(0);
            result.ErrorCount.ShouldBeGreaterThan(0);
            result.HasErrors.ShouldBeTrue(result.ListErrors());
            result.ResultObject.ShouldBeFalse();

            if (exactCount != null)
            {
                result.ErrorCount.ShouldBe((int)exactCount);
                result.Errors.Count.ShouldBe((int)exactCount);
            }
        }

        /// <summary>
        /// Assert that there are errors for the supplied property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result">Result under test</param>
        /// <param name="property">Key of the error to be asserted against</param>
        /// <param name="exactCount">When supplied, asserts the exact number of errors with the property. NOT total number of errors</param>
        /// <param name="containedInMessage">When supplied, asserts that the property's error message contains this string</param>
        public static void ShouldHaveErrorsFor<T>(this IResult<T> result, string property, int? exactCount = null, string containedInMessage = null)
        {
            result.ShouldNotBeNull();
            result.Errors.ShouldNotBeNull(ERROR_ERRORS_LIST_IS_NULL_MESSAGE);
            result.Errors.ShouldContain(e => e.Key == property, result.ListErrors());
            result.ErrorCount.ShouldBeGreaterThan(0);
            result.HasErrors.ShouldBeTrue();

            if (!string.IsNullOrWhiteSpace(containedInMessage))
            {
                containedInMessage = containedInMessage.ToLower();

                result.Errors.ShouldContain(e => e.Key == property && e.Message.ToLower().Contains(containedInMessage), result.ListErrors());
            }

            if (exactCount != null)
            {
                result.Errors.Where(e => e.Key == property).Count().ShouldBe((int)exactCount);
            }
        }

        /// <summary>
        /// Assert error exists for `ERROR_RESOURCE_NOT_FOUND_KEY`
        /// </summary>
        /// <param name="result">Result under test</param>
        /// <typeparam name="T"></typeparam>
        public static void ShouldHaveResourceNotFoundError<T>(this IResult<T> result) =>
            result.ShouldHaveErrorsFor(CoreErrorConstants.ERROR_RESOURCE_NOT_FOUND_KEY);

        /// <summary>
        /// Assert that there are no errors for the given result
        /// </summary>
        /// <param name="result">Result under test</param>
        /// <typeparam name="T"></typeparam>
        public static void ShouldNotHaveErrors<T>(this IResult<T> result)
        {
            result.Errors?.Count.ShouldBe(0, $"Unexpected errors: {result.ListErrors()}");
            result.ErrorCount.ShouldBe(0);
            result.HasErrors.ShouldBeFalse($"Unexpected errors: {result.ListErrors()}");
        }

        /// <summary>
        /// Assert that there weren't any errors for the supplied property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result">Result under test</param>
        /// <param name="property">Key of the error to be asserted against</param>
        public static void ShouldNotHaveErrorsFor<T>(this IResult<T> result, string property)
        {
            if (result.Errors == null || result.Errors.Count == 0)
            {
                return;
            }

            result.Errors.ShouldNotContain(e => e.Key == property);
        }

        #endregion Public Methods
    }
}
