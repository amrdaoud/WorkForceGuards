using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories;
using WorkForceManagementV0.Repositories.Interfaces;
using WorkForceGuards.Models.Interfaces;
using System.ComponentModel;
using OfficeOpenXml;
using System.IO;
using WorkForceGuards.Models.Reports;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ReportsController : ControllerBase
    {
        private readonly IReportAdheranceService _IReportAdheranceService;
        public  ReportsController(IReportAdheranceService ReportAdheranceService)
        {
            _IReportAdheranceService = ReportAdheranceService;
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("Adherance")]
        public ActionResult Report(ReportFilter model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Is Not Valid");
            }


            var action = _IReportAdheranceService.Report(model, User);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });

            }
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("Activities")]
        public IActionResult ActivityReport(DateTime RequestDate)
        {
            var action = _IReportAdheranceService.ActivityReport(RequestDate, User);
            if(string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
            
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("StaffAttendanceReport")]
        public IActionResult StaffAttendanceReport(StaffAttendanceFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Is Not Valid");
            }
            var action = _IReportAdheranceService.StaffAttendanceReport(filter, User);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });

            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("StaffWorkingDaysReport")]
        public IActionResult StaffWorkingDaysReport(StaffAttendanceFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Is Not Valid");
            }
            var action = _IReportAdheranceService.StaffWorkingDaysReport(filter, User);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });

            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("StaffAttendanceReport/download")]
        public IActionResult StaffAttendanceReportDonwload(StaffAttendanceFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Is Not Valid");
            }
            filter.PageIndex = 0;
            filter.PageSize = int.MaxValue;
            try
            {
                var action = _IReportAdheranceService.StaffAttendanceReportDownload(filter, User);
                FileBytesModel excelfile = new FileBytesModel();
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                var stream = new MemoryStream();
                var package = new ExcelPackage(stream);
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(action, true);
                package.Save();
                excelfile.Bytes = stream.ToArray();
                stream.Position = 0;
                stream.Close();
                string excelName = $"StaffWorkingDays.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                excelfile.FileName = excelName;
                excelfile.ContentType = contentType;
                return File(excelfile.Bytes, excelfile.ContentType, excelfile.FileName);
            }
            
            catch(Exception ex)
            {
                return BadRequest(new { ErrorMessage = ex.Message });
            }

            

        }
        [Authorize(Policy = "Admin")]
        [HttpPost("StaffWorkingDaysReport/download")]
        public IActionResult StaffWorkingDaysReportDownload(StaffAttendanceFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Is Not Valid");
            }
            filter.PageIndex = 0;
            filter.PageSize = int.MaxValue;
            try
            {
                var action = _IReportAdheranceService.StaffWorkingDaysReportDownload(filter, User);
                FileBytesModel excelfile = new FileBytesModel();
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                var stream = new MemoryStream();
                var package = new ExcelPackage(stream);
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(action, true);
                package.Save();
                excelfile.Bytes = stream.ToArray();
                stream.Position = 0;
                stream.Close();
                string excelName = $"StaffWorkingDays.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                excelfile.FileName = excelName;
                excelfile.ContentType = contentType;
                return File(excelfile.Bytes, excelfile.ContentType, excelfile.FileName);
            }

            catch (Exception ex)
            {
                return BadRequest(new { ErrorMessage = ex.Message });
            }



        }










    }
}
