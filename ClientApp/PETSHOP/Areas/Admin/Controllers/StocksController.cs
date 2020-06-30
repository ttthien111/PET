using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class StocksController : CheckAuthenticateManageController
    {
        public List<InventoryRecieveItem> IRItems
        {
            get
            {
                List<InventoryRecieveItem> inventoryRecieveItems = HttpContext.Session.GetObject<List<InventoryRecieveItem>>("inventoryRecieveItems");
                if (inventoryRecieveItems == default(List<InventoryRecieveItem>))
                {
                    inventoryRecieveItems = new List<InventoryRecieveItem>();
                }

                return inventoryRecieveItems;
            }
        }
        public IActionResult Index()
        {
            return View(IRItems);
        }

        public IActionResult Filter(string textSearch)
        {
            List<Product> filters = new List<Product>();
            if (textSearch == "" || textSearch == null)
            {
                filters = new List<Product>();
            }
            else
            {
                List<string> searchTexts = textSearch.Split(" ").ToList();
                foreach (string search in searchTexts)
                {
                    filters.AddRange(GetApiProducts.GetProducts()
                        .Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                p.ProductId.ToString() == search).ToList());
                }
            }
           
            return PartialView(filters);
        }

        public IActionResult AddToInventoryRecieveItems(int productId)
        {
            List<InventoryRecieveItem> inventoryRecieveItems = IRItems;
            Product product = GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId);
            string slugCat = GetApiCategories.GetCategories().SingleOrDefault(p => p.CategoryId == product.CategoryId).CategoryName;
            switch (slugCat)
            {
                case Constants.FOOD:
                    inventoryRecieveItems.AddRange(GetApiFoodProducts.GetFoodProducts().Where(p => p.ProductId == product.ProductId)
                                        .Select(p=> new InventoryRecieveItem() {
                                            ProductId = product.ProductId,
                                            SubProductId = p.FoodId,
                                            ProductName = product.ProductName,
                                            Size = "",
                                            Amount = 0,
                                            Price = product.ProductPrice
                                        }).ToList());
                    break;

                case Constants.TOY:
                    inventoryRecieveItems.AddRange(GetApiToyProducts.GetToyProducts().Where(p => p.ProductId == product.ProductId)
                                        .Select(p => new InventoryRecieveItem()
                                        {
                                            ProductId = product.ProductId,
                                            SubProductId = p.ToyId,
                                            ProductName = product.ProductName,
                                            Size = "",
                                            Amount = 1,
                                            Price = product.ProductPrice
                                        }).ToList());
                    break;

                case Constants.COSTUME:
                    inventoryRecieveItems.AddRange(GetApiCostumeProducts.GetCostumeProducts().Where(p => p.ProductId == product.ProductId)
                                        .Select(p => new InventoryRecieveItem()
                                        {
                                            ProductId = product.ProductId,
                                            SubProductId = p.CostumeId,
                                            ProductName = product.ProductName,
                                            Size = p.CostumeSize,
                                            Amount = 1,
                                            Price = product.ProductPrice
                                        }).ToList());
                    break;
            }

            HttpContext.Session.SetObject("inventoryRecieveItems", inventoryRecieveItems);
            return PartialView(inventoryRecieveItems);
        }

        public IActionResult UpdateInventoryRecieveNote(string updateModel)
        {
            InventoryRecieveItemUpdateModel update = JsonConvert.DeserializeObject<InventoryRecieveItemUpdateModel>(updateModel);
            List<InventoryRecieveItem> inventoryRecieveItems = IRItems;
            InventoryRecieveItem inventoryRecieveItem = new InventoryRecieveItem();
            if (update.Size == "" || update.Size == null)
            {
                inventoryRecieveItem = inventoryRecieveItems
                                        .SingleOrDefault(p => p.ProductId == update.ProductId &&
                                                              p.SubProductId == update.SubProductId);
            }
            else
            {
                inventoryRecieveItem = inventoryRecieveItems
                                        .SingleOrDefault(p => p.ProductId == update.ProductId &&
                                                              p.SubProductId == update.SubProductId &&
                                                              p.Size == update.Size);
            }
             
            inventoryRecieveItem.Price = update.Price;
            inventoryRecieveItem.Amount = update.Amount;

            HttpContext.Session.SetObject("inventoryRecieveItems", inventoryRecieveItems);
            return NoContent();
        }

        public IActionResult DeleteInvetoryReceiveItem(int ProductId, int SubProductId)
        {
            List<InventoryRecieveItem> inventoryRecieveItems = IRItems;
            InventoryRecieveItem inventoryRecieveItem = inventoryRecieveItems
                                                        .SingleOrDefault(p => p.ProductId == ProductId &&
                                                                            p.SubProductId == SubProductId);

            inventoryRecieveItems.Remove(inventoryRecieveItem);
            HttpContext.Session.SetObject("inventoryRecieveItems", inventoryRecieveItems);
            return NoContent();
        }

        [HttpPost]
        public IActionResult SaveInventoryReceiveNote()
        {
            // get credential
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));

            //prepair data
            List<InventoryRecieveItem> inventoryRecieveItems = IRItems;
            InventoryReceiveNoteModel inventoryReceivingNote = new InventoryReceiveNoteModel()
            {
                InventoryReceivingDateReceiving = DateTime.Now,
                InventoryReceivingTotalPrice = inventoryRecieveItems.Sum(p => p.Amount * p.Price)
            };

            // create inventory Receive Note
            InventoryReceivingNote createdReceivingNote = CreateInventoryReceiveNote(inventoryReceivingNote, credential.JwToken);

            // get list toy, food, costume
            List<InventoryReceivingNoteDetailForFood> noteDetailForFoods = inventoryRecieveItems
                        .Where(p => IsFood(p.ProductId))
                        .Select(p=> new InventoryReceivingNoteDetailForFood() {
                            InventoryReceivingId = createdReceivingNote.InventoryReceivingId,
                            FoodProductId = GetApiFoodProducts.GetFoodProducts()
                            .SingleOrDefault(k =>k.ProductId == p.ProductId).FoodId,
                            FoodProductAmount = p.Amount
                        }).ToList();

            List<InventoryReceivingNoteDetailForToy> noteDetailForToys = inventoryRecieveItems
                        .Where(p => IsToy(p.ProductId))
                        .Select(p => new InventoryReceivingNoteDetailForToy()
                        {
                            InventoryReceivingId = createdReceivingNote.InventoryReceivingId,
                            ToyProductId = GetApiToyProducts.GetToyProducts()
                            .SingleOrDefault(k => k.ProductId == p.ProductId).ToyId,
                            ToyProductAmount = p.Amount
                        }).ToList();

            List<InventoryReceivingNoteDetailForCostume> noteDetailForCostumes = inventoryRecieveItems
                        .Where(p => IsCostume(p.ProductId))
                        .Select(p => new InventoryReceivingNoteDetailForCostume()
                        {
                            InventoryReceivingId = createdReceivingNote.InventoryReceivingId,
                            CostumeProductId = GetApiCostumeProducts.GetCostumeProducts()
                            .SingleOrDefault(k => k.ProductId == p.ProductId && k.CostumeSize == p.Size).CostumeId,
                            CostumeProductAmount = p.Amount,
                            CostumeProductSize = p.Size
                        }).ToList();

            // create IR food list
            foreach (var food in noteDetailForFoods)
            {
                //create
                GetApiInventoryReceivingNoteDetailForFoods.Post(food, credential.JwToken);
                //update amount for product
                FoodProduct foodProduct = GetApiFoodProducts.GetFoodProducts()
                                            .SingleOrDefault(p => p.FoodId == food.FoodProductId);
                foodProduct.FoodAmount += food.FoodProductAmount;

                GetApiFoodProducts.UpdateStock(foodProduct, credential.JwToken);
            }

            // create IR toy list
            foreach (var toy in noteDetailForToys)
            {
                GetApiInventoryReceiveNoteDetailForToys.Post(toy, credential.JwToken);
                //update amount for product
                ToyProduct toyProduct = GetApiToyProducts.GetToyProducts()
                                            .SingleOrDefault(p => p.ToyId == toy.ToyProductId);
                toy.ToyProductAmount += toy.ToyProductAmount;

                GetApiToyProducts.UpdateStock(toyProduct, credential.JwToken);
            }

            // create IR costume list
            foreach (var costume in noteDetailForCostumes)
            {
                GetApiInventoryReceiveNoteDetailForCostumes.Post(costume, credential.JwToken);
                //update amount for product
                CostumeProduct costumeProduct = GetApiCostumeProducts.GetCostumeProducts()
                                            .SingleOrDefault(p => p.CostumeId == costume.CostumeProductId &&
                                                                  p.CostumeSize == costume.CostumeProductSize);
                costumeProduct.CostumeAmountSize += costume.CostumeProductAmount;

                GetApiCostumeProducts.UpdateStock(costumeProduct, credential.JwToken);
            }

            HttpContext.Session.Remove("inventoryRecieveItems");
            return Json(createdReceivingNote);
        }

        private InventoryReceivingNote CreateInventoryReceiveNote(InventoryReceiveNoteModel inventoryReceivingNote, string token)
        {
            return GetApiInventoryReceiveNotes.Post(inventoryReceivingNote, token);
        }

        private bool IsCostume(int productId)
        {
            Category category = GetApiCategories.getCategoryBySlugName(Constants.COSTUME);
            return GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId && p.CategoryId == category.CategoryId) != null;
        }

        private bool IsToy(int productId)
        {
            Category category = GetApiCategories.getCategoryBySlugName(Constants.TOY);
            return GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId && p.CategoryId == category.CategoryId) != null;
        }

        private bool IsFood(int productId)
        {
            Category category = GetApiCategories.getCategoryBySlugName(Constants.FOOD);
            return GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId && p.CategoryId == category.CategoryId) != null;
        }
    }
}   