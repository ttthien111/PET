using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiCategories
    {
        public static IEnumerable<Category> GetCategories()
        {
            IEnumerable<Category> categories = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync("categories");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Category>>();
                    readTask.Wait();

                    categories = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    categories = Enumerable.Empty<Category>();
                }
            }

            return categories;
        }
        public static Category getCategoryBySlugName(string slugCat)
        {
            Category category = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync("categories/" + slugCat);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Category>();
                    readTask.Wait();

                    category = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    category = new Category();
                }
            }

            return category;
        }

    }
}
