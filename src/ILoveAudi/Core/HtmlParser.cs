using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ILoveAudi.Controllers;
using ILoveAudi.Models;
using ILoveAudi.Services;
using Microsoft.EntityFrameworkCore;

namespace ILoveAudi.Core
{
   
    public class HtmlParser:IHtmlParser
    {
        ILoveAudiContext _db;

        public HtmlParser(ILoveAudiContext db)
        {
            _db = db;
        }

        public async Task<List<Car>> SaveCarToDb()
        {
           
            var cars = await GetAllCarsFromShop();
            await _db.Cars.AddRangeAsync(cars);
            await _db.SaveChangesAsync();
            return cars;

        }

        public List<Car> Cars()
        {
            //я не смог понять почему запрос с помощью ef из базы не даёт результата увы
            var cars = _db.Cars.Where(x=>x.Name=="AudiA6");
            var carsss = cars.ToList();
            return new List<Car>();
        }

        
        public async Task<List<Car>> GetAllCarsFromShop()
        {
            var pageCount = await GetPagesCount();
            var allCars = new List<Car>();

            for (var i = 1; i <= pageCount; i++)
            {
                var cars = await GetCarsFromPage(i.ToString());
                allCars = allCars.Concat(cars).ToList();
            }
            return allCars;
        }
        private async Task<int> GetPagesCount()
        {
            string page = "https://auto.e1.ru/car/audi/?region%5B0%5D=213&not_sold=1&limit=50";

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                var html = new HtmlDocument();
                html.LoadHtml(result);
                return PageCount(html);
            }
        }
        private int PageCount(HtmlDocument node)
        {
            var divWithLi = node.DocumentNode.Descendants("div")
                .First(
                    x =>
                        x.Attributes.Contains("class") &&
                        x.Attributes["class"].Value.Contains("au-pagination"));
            var liValue = divWithLi.Descendants("li")
                .Last(
                    x =>
                        x.Attributes.Contains("class") &&
                        x.Attributes["class"].Value.Contains("au-pagination__item"));
            var number = int.Parse(liValue.Element("a").InnerText);

            return number;

        }
        private async Task<List<Car>> GetCarsFromPage(string pageNumber)
        {
            string page = "https://auto.e1.ru/car/audi/?region%5B0%5D=213&not_sold=1&limit=50&page=" + pageNumber;

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(page))
            using (var content = response.Content)
            {

                string result = await content.ReadAsStringAsync();
                var html = new HtmlDocument();
                html.LoadHtml(result);

                var carLiElements = CarLiElements(html);
                var cars = new List<Car>();

                foreach (var carElement in carLiElements)
                {
                    var car = GetCar(carElement);
                    cars.Add(car);
                }
                return cars;
            }
        }
        private IEnumerable<HtmlNode> CarLiElements(HtmlDocument html)
        {
            return html.DocumentNode.Descendants("li")
                .Where(
                    d =>
                        d.Attributes.Contains("class") &&
                        d.Attributes["class"].Value.Contains("au-offers__item _offers_item ")
                );
        }
        private Car GetCar(HtmlNode carElement)
        {
            var name =
                TextFromNode(carElement, "a", "au-offers__item-title")
                    .Replace(" ", "")
                    .Replace("\n", "");

            var year = int.Parse(
                TextFromNode(carElement, "span", "au-offers__item-title"));

            var price = int.Parse(
                TextFromNode(carElement, "span", "au-offers__item-price _item_price")
                    .Replace(" ", "")
                    .Replace("\n", "")
                    .Replace("&nbsp;", "")
                    .Replace("P", ""));

            return new Car(name, year, price);
        }
        private string TextFromNode(HtmlNode node, string tagName, string className)
        {
            return node.Descendants(tagName)
                .First(
                    x =>
                        x.Attributes.Contains("class") &&
                        x.Attributes["class"].Value.Contains(className)).InnerText;

        }
    }
}