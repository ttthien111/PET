using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiInventoryReceivingNoteDetailForFoods
    {
        public static IEnumerable<InventoryReceivingNoteDetailForFood> GetInventoryReceivingNoteDetailForFoods(string token)
        {
            IEnumerable<InventoryReceivingNoteDetailForFood> res = null;
            using (var client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.INVENTORY_RECEIVE_NOTE_FOOD);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<InventoryReceivingNoteDetailForFood>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<InventoryReceivingNoteDetailForFood>();
                }
            }

            return res;
        }

        public static InventoryReceivingNoteDetailForFood Post(
            InventoryReceivingNoteDetailForFood inventoryReceivingNoteDetailForFood,
            string token
            )
        {
            using (var client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client
                    .PostAsJsonAsync<InventoryReceivingNoteDetailForFood>(Constants.INVENTORY_RECEIVE_NOTE_FOOD, inventoryReceivingNoteDetailForFood);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<InventoryReceivingNoteDetailForFood>();
                    readTask.Wait();

                    return readTask.Result;
                }
                return null;
            }
        }
    }
}
