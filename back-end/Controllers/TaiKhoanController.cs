﻿using Azure;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly ITaiKhoanService _accountService;

        public TaiKhoanController(ITaiKhoanService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 6, [FromQuery] string searchString = "")
        {
            var response = await _accountService.GetAllAccounts(pageIndex, pageSize, searchString ?? "");
            return Ok(response);
        }

        [Authorize]
        [HttpGet("ngoai-tru-user-dang-nhap")]
        public async Task<IActionResult> GetAllExceptLoggedInUser()
        {
            var response = await _accountService.GetAllContactAccounts();
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById([FromRoute] string id)
        {
            var response = await _accountService.GetUserById(id);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("quan-tri-vien")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var response = await _accountService.GetAllAdminAccounts();
            return Ok(response);
        }

        [Authorize]
        [HttpGet("user-dang-nhap")]
        public async Task<IActionResult> GetUserDetails()
        {
            var response = await _accountService.GetUserDetails();
            return Ok(response);
        }

        [HttpPost("doi-mat-khau")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var response = await _accountService.ChangePassword(request);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("tao-tai-khoan")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRequest request)
        {
            var response = await _accountService.CreateAccount(request);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("cap-nhat-tai-khoan/{id}")]
        public async Task<IActionResult> EditAccount([FromRoute] string id, [FromBody] EditAccountRequest request)
        {
            await _accountService.UpdateAccount(id, request);
            return NoContent();
        }

        [Authorize]
        [HttpPost("cap-nhat-ho-so")]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var response = await _accountService.UpdateProfile(request);
            return response;
        }

        [Authorize]
        [HttpPost("cap-nhat-hinh-dai-dien")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
        {
            var response = await _accountService.UploadAvatar(file);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("cap-nhat-hinh-bia")]
        public async Task<IActionResult> UploadCoverImage([FromForm] IFormFile file)
        {
            var response = await _accountService.UploadCoverImage(file);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("yeu-cau-khoi-phuc-mat-khau")]
        public async Task<IActionResult> RequestResetPassword([FromQuery] string email)
        {
            var response = await _accountService.RequestPasswordReset(email);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("khoi-phuc-mat-khau")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var response = await _accountService.ResetPassword(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("xac-thuc-token")]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenRequest request)
        {
            var response = await _accountService.ValidateToken(request);
            return Ok(response);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("khoa/{id}")]
        public async Task<IActionResult> LockAccount([FromRoute] string id)
        {
            await _accountService.LockAccount(id);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("mo-khoa/{id}")]
        public async Task<IActionResult> UnlockAccount([FromRoute] string id)
        {
            await _accountService.UnlockAccount(id);
            return NoContent();
        }
    }
}
