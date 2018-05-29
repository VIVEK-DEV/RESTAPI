using GoodRestApiWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace GoodRestApiWebMvc.Controllers
{
    public class HomeController : Controller
    {
        public static  List<RootObject> storedlist;
        public async Task<ActionResult> Index(int? page)
        {
            List<RootObject> FeedList;
           ApiHelper apihelper = new ApiHelper();
            
            if (storedlist == null)
            {
                 FeedList = await apihelper.GetAPIData("", "", "");
                storedlist = FeedList;
            }
            else {
                 FeedList = storedlist;
            }
            var FeedCount = FeedList.Count();
            ViewBag.PageCount = FeedCount / 100;
            ViewBag.Pageno = page;
            ViewBag.FeedsCount = FeedCount;
            int pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<RootObject> feedlist = new List<RootObject>();
            var test = FeedList.ToPagedList(pageindex, 10);




            return View(test);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}