using back_end.Core.Models;
using back_end.Core.Requests;
using back_end.Core.Responses;
using back_end.Core.Responses.Resources;
using back_end.Data;
using back_end.Exceptions;
using back_end.Mappers;
using back_end.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace back_end.Services.Implements
{
    public class DiaChiGiaoHangService : IDiaChiGiaoHangService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly MyStoreDbContext dbContext;
        private readonly ApplicationMapper _applicationMapper;

        public DiaChiGiaoHangService(IHttpContextAccessor contextAccessor, MyStoreDbContext dbContext, ApplicationMapper applicationMapper)
        {
            _contextAccessor = contextAccessor;
            this.dbContext = dbContext;
            this._applicationMapper = applicationMapper;
        }

        private async Task setDefaultToFalse()
        {
            DiaChiGiaoHang? defaultAddress = await dbContext.DiaChiGiaoHangs.
                SingleOrDefaultAsync(d => d.MacDinh);

            if (defaultAddress == null) return;
            defaultAddress.MacDinh = false;
        }

        public async Task<BaseResponse> CreateAddressOrder(AddressOrderRequest request)
        {
            if (request.IsDefault)
                await setDefaultToFalse();

            DiaChiGiaoHang addressOrder = new DiaChiGiaoHang();
            addressOrder.DiaChi = request.Address;
            addressOrder.SoDienThoai = request.PhoneNumber;
            addressOrder.HoVaTen = request.FullName;
            addressOrder.Email = request.Email;
            addressOrder.MacDinh = request.IsDefault;
            addressOrder.MaNguoiDung = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;

            var savedAddressOrder = await dbContext.DiaChiGiaoHangs.AddAsync(addressOrder);
            await dbContext.SaveChangesAsync();

            var response = new DataResponse<AddressOrderResource>();
            response.Message = "Thêm địa chỉ mới thành công";
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.Created;
            response.Data = _applicationMapper.MapToAddressOrderResource(savedAddressOrder.Entity);

            return response;
        }

        public async Task<BaseResponse> GetAllByUsers()
        {
            var user = _contextAccessor.HttpContext?.User;
            string userId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            List<DiaChiGiaoHang> addressOrders = await dbContext.DiaChiGiaoHangs
                .Where(a => a.MaNguoiDung == userId).ToListAsync();

            var response = new DataResponse<List<AddressOrderResource>>();
            response.Message = "Lấy danh sách địa chỉ thành công";
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.Created;
            response.Data = addressOrders.Select(addressOrder => _applicationMapper.MapToAddressOrderResource(addressOrder)).ToList();

            return response;

        }

        public async Task<BaseResponse> SetCheckedDefault(int id)
        {
            await setDefaultToFalse();
            DiaChiGiaoHang? addressOrder = await dbContext.DiaChiGiaoHangs
                .SingleOrDefaultAsync(a => a.MaDCGH == id)
                    ?? throw new NotFoundException("Địa chỉ không tồn tại");

            addressOrder.MacDinh = true;
            await dbContext.SaveChangesAsync();

            var response = new BaseResponse();
            response.Message = "Cập nhật trạng thái địa chỉ thành công";
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.NoContent;

            return response;
        }
    }
}
