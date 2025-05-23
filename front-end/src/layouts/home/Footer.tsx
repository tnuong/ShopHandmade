import { FC } from "react";
import { FaFacebookF, FaInstagram, FaTiktok } from "react-icons/fa";

const Footer: FC = () => {
  return (
    <footer className="bg-[#14131A] text-white pt-12 pb-6 mt-20">
      <div className="max-w-screen-xl mx-auto px-6 md:px-10 grid grid-cols-1 md:grid-cols-4 gap-10">

        {/* Thương hiệu & giới thiệu */}
        <div>
          <h2 className="text-2xl font-bold mb-4">
            HAND<span className="text-red-500">MADE</span>
          </h2>
          <p className="text-sm text-gray-400 leading-relaxed">
            Chuyên cung cấp các sản phẩm thủ công chất lượng cao được làm bằng sự tận tâm và sáng tạo.
          </p>
        </div>

        {/* Danh mục chính */}
        <div>
          <h3 className="text-lg font-semibold mb-3">Danh mục</h3>
          <ul className="text-sm text-gray-400 space-y-2">
            <li><a href="#">Trang chủ</a></li>
            <li><a href="#">Cửa hàng</a></li>
            <li><a href="#">Blog</a></li>
            <li><a href="#">Giới thiệu</a></li>
          </ul>
        </div>

        {/* Chính sách hỗ trợ */}
        <div>
          <h3 className="text-lg font-semibold mb-3">Hỗ trợ</h3>
          <ul className="text-sm text-gray-400 space-y-2">
            <li><a href="#">Chính sách vận chuyển</a></li>
            <li><a href="#">Chính sách đổi trả</a></li>
            <li><a href="#">Hướng dẫn mua hàng</a></li>
            <li><a href="#">Liên hệ</a></li>
          </ul>
        </div>

        {/* Mạng xã hội */}
        <div>
          <h3 className="text-lg font-semibold mb-3">Kết nối với chúng tôi</h3>
          <div className="flex items-center gap-4 mt-2 text-lg">
            <a href="#" className="hover:text-red-500"><FaFacebookF /></a>
            <a href="#" className="hover:text-red-500"><FaInstagram /></a>
            <a href="#" className="hover:text-red-500"><FaTiktok /></a>
          </div>
        </div>
      </div>

      {/* Bản quyền */}
      <div className="text-center text-gray-500 text-sm mt-10 border-t border-gray-700 pt-6">
        © {new Date().getFullYear()} HANDMADE. All rights reserved.
      </div>
    </footer>
  );
};

export default Footer;
