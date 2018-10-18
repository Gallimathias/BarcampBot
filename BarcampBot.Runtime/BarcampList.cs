using BarcampBot.Core;
using BarcampBot.Database;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcampBot.Runtime
{
    public class BarcampList : List<Barcamp>
    {
        public BarcampList(HtmlDocument document)
        {
            var list = document
                .GetElementbyId("liste")
                .ChildNodes["div"]
                .ChildNodes["div"]
                .ChildNodes
                .Where(h => h.HasClass("item"));

            ConvertList(list);
        }

        private void ConvertList(IEnumerable<HtmlNode> list)
        {
            foreach (var item in list)
                Add(ConvertItem(item));
        }

        private Barcamp ConvertItem(HtmlNode item)
        {
            var body = item.ChildNodes["p"];
            return new Barcamp()
            {
                Titel = item.ChildNodes["h3"].FirstChild.InnerHtml.Trim(),
                Cost = GetCosts(item.ChildNodes["h3"].ChildNodes["img"].Attributes["title"].Value),
                Time = GetTime(body.ChildNodes[0].InnerHtml.Trim()),
                Description = body.ChildNodes[2].InnerHtml.Trim(),
                Link = body.ChildNodes.FirstOrDefault(h => h.InnerHtml == "Webseite")?.Attributes["href"].Value.Trim(),
                Twitter = body.ChildNodes.FirstOrDefault(h => h.InnerHtml == "Twitter")?.Attributes["href"].Value.Trim(),
                Facebook = body.ChildNodes.FirstOrDefault(h => h.InnerHtml == "Facebook")?.Attributes["href"].Value.Trim(),
                Hashtag = body.ChildNodes.FirstOrDefault(h => h.InnerHtml.StartsWith("#"))?.InnerHtml.Trim()
            };
        }

        private LocationTime GetTime(string locationTimeString)
            => LocationTime.Parse(locationTimeString);

        private Barcamp.CostCategory GetCosts(string title)
        {
            title = title.Replace("&#40;", "(").Replace("&#41;", ")").Trim().ToLower();
            var index = title.IndexOf('(');

            switch (title.Substring(0, index < 0 ? title.Length : index).Trim())
            {
                case "kostenfrei":
                    return Barcamp.CostCategory.Free;
                case "kostenbeitrag":
                    return Barcamp.CostCategory.Post;
                case "kostenpflichtig":
                    return Barcamp.CostCategory.Duty;
                default:
                    return Barcamp.CostCategory.None;
            }
        }
    }
}
