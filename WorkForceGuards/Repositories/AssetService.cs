using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class AssetService: IAssetService
    {
        private readonly ApplicationDbContext db;
        public AssetService(ApplicationDbContext context)
        {
            db = context;
        }
        public List<AssetDto> GetAll()
        {
            var result = db.Assets.Select(x => new AssetDto
            {
                Id = x.Id,
                Name=x.Name,
                Barcode=x.Barcode,
                TypeName=x.AssetType!=null?x.AssetType.Name:"Deleted",
                LocationName=x.Location!=null ? x.Location.Name:"Deleted",
                LocationId=x.LocationId,
                TypeId=x.AssetTypeId,
                Specs=x.Specs,
                IsDisabled=x.IsDisabled
            }).ToList();

            return result;
        }


        public FullAssetDto GetAll(FilterModel f)
        {
            var asset = db.Assets.Include(x => x.Location).Include(x => x.AssetType).AsQueryable();

            if (!string.IsNullOrEmpty(f.SearchQuery))
            {
                if(int.TryParse(f.SearchQuery,out var id))
                {
                    asset = asset.Where(x => x.Id == id);
                }
                else
                {
                    asset = asset.Where(r => r.Name.ToLower().StartsWith(f.SearchQuery.ToLower())||
                    r.Barcode.ToLower().StartsWith(f.SearchQuery.ToLower()));
                }
            }
            var q = asset.ToList();
            if(f.Filters.Count!=0)
            {
                foreach (Filter c in f.Filters)
                {
                    var filterKeyProperty = typeof(Asset).GetProperty(c.Key);
                    q =  q.Where(x => c.Values.Contains(filterKeyProperty.GetValue(x).ToString())).ToList();

                }
            }

            


            var result = q.Select(x => new AssetDto
            {
                Id = x.Id,
                Name = x.Name,
                Barcode = x.Barcode,
                TypeName = x.AssetType != null ? x.AssetType.Name : "Deleted",
                LocationName = x.Location != null ? x.Location.Name : "Deleted",
                LocationId = x.LocationId,
                TypeId = x.AssetTypeId,
                Specs = x.Specs,
                IsDisabled = x.IsDisabled
            }).ToList();

            FullAssetDto ResultDto = new FullAssetDto();

            if (!string.IsNullOrEmpty(f.Sort))

            {
                List<AssetDto> data = new List<AssetDto>();

                var sortProperty = typeof(AssetDto).GetProperty(f.Sort);
                if (sortProperty != null && f.Order == "asc")
                {
                    data = result.OrderBy(x => sortProperty.GetValue(x)).ToList();
                    data = data.Skip(f.PageIndex * f.PageSize).Take(f.PageSize).ToList();
                }

                else if (sortProperty != null && f.Order == "desc")
                {
                    data = result.OrderByDescending(x => sortProperty.GetValue(x)).ToList();
                    data = data.Skip(f.PageIndex * f.PageSize).Take(f.PageSize).ToList();
                }



                ResultDto.Result = data;
                ResultDto.ResultSize = result.Count;

                return ResultDto;
            }

            else
            {
                List<AssetDto> data = new List<AssetDto>();

               
                    data = result.Skip(f.PageIndex * f.PageSize).Take(f.PageSize).ToList();
                
                ResultDto.Result = data;
                ResultDto.ResultSize = result.Count;

                return ResultDto;
            }


           

        }


   
        public AssetDto GetById(int id)
        {
            var result = db.Assets.Select(x => new AssetDto
            {
                Id = x.Id,
                Name = x.Name,
                Barcode = x.Barcode,
                TypeName = x.AssetType != null ? x.AssetType.Name : "Deleted",
                LocationName = x.Location != null ? x.Location.Name : "Deleted",
                LocationId = x.LocationId,
                TypeId = x.AssetTypeId,
                Specs = x.Specs,
                IsDisabled = x.IsDisabled
            }).FirstOrDefault(x=>x.Id==id);

            return result;


        }



        public DataWithError Add(Asset model)
        {
            DataWithError Data = new DataWithError();

            if (CheckUniqValue(model))
            {
                db.Assets.Add(model);
                db.SaveChanges();

                var result = db.Assets.Select(x => new AssetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Barcode = x.Barcode,
                    TypeName = x.AssetType.Name,
                    LocationName = x.Location.Name,
                    LocationId = x.LocationId,
                    TypeId = x.AssetTypeId,
                    Specs = x.Specs,
                    IsDisabled = x.IsDisabled
                }).FirstOrDefault(x => x.Id == model.Id);

                Data.Result = result;
                Data.ErrorMessage = null;
                return Data;

             }
             else
            {
                Data.Result = null;
                Data.ErrorMessage = "Duplicated Barcode Inserted";
                return Data;
            }
                

             
           
        }


        public DataWithError Update(Asset model)
        {
            DataWithError data = new DataWithError();

                if(CheckUniqValue(model))
                {

                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                    var result = db.Assets.Select(x => new AssetDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Barcode = x.Barcode,
                        TypeName = x.AssetType.Name,
                        LocationName = x.Location.Name,
                        LocationId = x.LocationId,
                        TypeId = x.AssetTypeId,
                        Specs = x.Specs,
                        IsDisabled = x.IsDisabled
                    }).FirstOrDefault(c => c.Id == model.Id);

                    data.Result = result;
                    data.ErrorMessage = null;
                    return data;
                }


            data.Result = null;
            data.ErrorMessage = "Duplicated Barcode Inserted";
            return data;


        }


        public bool Delete(int id)
        {
            var asset = db.Assets.Find(id);
            db.Assets.Remove(asset);
            db.SaveChanges();
            return true;
        }


        public bool CheckUniqValue(Asset value)
        {
            var same = db.Assets.FirstOrDefault(a => a.Barcode.ToLower() == value.Barcode.ToLower() && a.Id != value.Id);
            if(same == null)
            {
                return true;
            }
            return false;
           
        }


        public bool CheckValue(string value)
        {
            var same = db.Assets.FirstOrDefault(a => a.Barcode.ToLower() == value.ToLower());
            if (same == null)
            {
                return true;
            }
            return false;
        }

    }
}
