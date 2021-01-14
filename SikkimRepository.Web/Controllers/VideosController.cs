using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SikkimRepository.Web.Controllers
{
    public class VideosController : Controller
    {
        dalVideoAlbum ObjVideoAlbum = new dalVideoAlbum();
        dalVideo ObjVideo = new dalVideo();
        dalComponent ObjComp = new dalComponent();
        // GET: Video
        public ActionResult Index(long SCID = 0, int PageNo = 1, int PageSize = 3, string searchTerm = "")
        {
            VideoAlbumManageModel model = new VideoAlbumManageModel();
            VideoAlbumGenViewModel _vmodel = new VideoAlbumGenViewModel();
            _vmodel = ObjVideoAlbum.GenVideoAlbumSelectPaged(SCID, PageNo, PageSize, searchTerm);
            model.GenVideoAlbums = _vmodel;
            model.Components = ObjComp.GetComponentList("");
            model.GenComponentModelList = ObjComp.GetGenComponentList();//Component List
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvVideoAlbumList", model);
            }
            return View(model);
        }
        public ActionResult VideoList(long id)
        {
            VideoAlbumManageModel model = new VideoAlbumManageModel();
            utblVideoAlbum _vmodel = new utblVideoAlbum();
            model.Videolist = ObjVideo.GetVideoByAlbumID(id);
            _vmodel = ObjVideoAlbum.GetVideoAlbumByID(id);
            model.VideoAlbum = _vmodel;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvVideoList", model);
            }
            return View(model);

        }
    }
}