using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiInventoryReceiveNoteDetailForCostumes
    {
        public static IEnumerable<InventoryReceivingNoteDetailForCostume> GetInventoryReceivingNoteDetailForCostumes(string token)
        {
            IEnumerable<InventoryReceivingNoteDetailForCostume> res = null;
            using (var client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.INVENTORY_RECEIVE_NOTE_COSTUME);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<InventoryReceivingNoteDetailForCostume>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<InventoryReceivingNoteDetailForCostume>();
                }
            }

            return res;
        }

        public static InventoryReceivingNoteDetailForCostume Post(
            InventoryReceivingNoteDetailForCostume inventoryReceivingNoteDetailForCostume,
            string token
            )
        {
            using (var client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client
                    .PostAsJsonAsync<InventoryReceivingNoteDetailForCostume>(Constants.INVENTORY_RECEIVE_NOTE_COSTUME, inventoryReceivingNoteDetailForCostume);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<InventoryReceivingNoteDetailForCostume>();
                    readTask.Wait();

                    return readTask.Result;
                }
                return null;
            }
        }
    }
}
