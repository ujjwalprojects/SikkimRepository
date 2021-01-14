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
    public class ImagesController : Controller
    {
        dalImageAlbum ObjImgAlbum = new dalImageAlbum();
        dalImage ObjImage = new dalImage();
        dalComponent ObjComp = new dalComponent();
        // GET: Images
        public ActionResult Index(long SCID = 0, int PageNo = 1, int PageSize = 3, string searchTerm = "")
        {
            ImageAlbumManageModel model = new ImageAlbumManageModel();
            ImageAlbumGenViewModel _imodel = new ImageAlbumGenViewModel();
            _imodel = ObjImgAlbum.GenImageAlbumSelectPaged(SCID, PageNo, PageSize, searchTerm);
            model.GenImageAlbums = _imodel;
            model.Components = ObjComp.GetComponentList("");
            model.GenComponentModelList = ObjComp.GetGenComponentList();//Component List
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvImageAlbumList", model);
            }
            return View(model);
        }
        public ActionResult Photo(long id)
        {
            ImageAlbumManageModel model = new ImageAlbumManageModel();
            utblImageAlbum _imodel = new utblImageAlbum();
            model.Imageslist = ObjImage.GetImageByAlbumID(id);
            _imodel = ObjImgAlbum.GetImgAlbumByID(id);
            model.ImageAlbum = _imodel;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvImageList", model);
            }
            return View(model);

        }
    }
}