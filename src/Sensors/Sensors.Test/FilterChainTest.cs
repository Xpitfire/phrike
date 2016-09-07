// <summary>Unit-Test for FilterChain</summary>
// -----------------------------------------------------------------------
// Copyright (c) 2015 University of Applied Sciences Upper-Austria
// Project OperationPhrike
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------------------------
namespace Sensors.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Phrike.Sensors.Filters;

    /// <summary>
    /// The filter chain test.
    /// </summary>
    [TestClass]
    public class FilterChainTest
    {
        /// <summary>
        /// The unfiltered.
        /// </summary>
        private readonly double[] unfilteredOne =
            new double[] { 6, 0, 0, 9, 0, 0, 0, 5, 0, 0, 0, 0, 0, 7 };

        /// <summary>
        /// The unfiltered two.
        /// </summary>
        private readonly double[] unfilteredTwo =
            new double[] { 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 };

        /// <summary>
        /// The fc filter.
        /// </summary>
        [TestMethod]
        public void FcConstructorFilter()
        {
            FilterChain filterChain = new FilterChain(new IFilter[]
                                    {
                                        new FixedResultFilter(unfilteredOne),
                                    });

            CollectionAssert.AreEqual(filterChain.Filter(unfilteredOne).ToArray(), unfilteredOne);
        }

        /// <summary>
        /// The fc filter.
        /// </summary>
        [TestMethod]
        public void FcConstructorListFilter()
        {
            FilterChain filterChain = new FilterChain(new IFilter[]
                                    {
                                        new FixedResultFilter(unfilteredOne),
                                        new FixedResultFilter(unfilteredTwo)
                                    });

            CollectionAssert.AreEqual(filterChain.Filter(unfilteredOne).ToArray(), unfilteredTwo);
        }

        /// <summary>
        /// The fc add filter.
        /// </summary>
        [TestMethod]
        public void FcAddFilter()
        {
            IFilter[] filters = new IFilter[]
                                    {
                                        new FixedResultFilter(unfilteredOne)
                                    };

            FilterChain filterChain = new FilterChain(filters);
            filterChain.Add(new FixedResultFilter(unfilteredTwo));

            CollectionAssert.AreEqual(filterChain.Filter(unfilteredOne).ToArray(), unfilteredTwo);
        }
    }
}