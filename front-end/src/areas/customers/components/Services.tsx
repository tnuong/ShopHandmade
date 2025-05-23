import { faArrowsRotate, faMessage, faTruck } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FC } from "react";

const services = [
  {
    icon: faTruck,
    title: "Miễn phí vận chuyển",
    description:
      "Chúng tôi rất vui được thông báo rằng đơn hàng gần đây của bạn đủ điều kiện để nhận ưu đãi miễn phí vận chuyển!",
  },
  {
    icon: faArrowsRotate,
    title: "Hoàn trả miễn phí",
    description:
      "Hoàn trả miễn phí trong vòng 10 ngày, vui lòng đảm bảo sản phẩm còn nguyên vẹn, không hư hỏng.",
  },
  {
    icon: faMessage,
    title: "Hỗ trợ 24/7",
    description:
      "Chúng tôi hỗ trợ khách hàng 24/7. Gửi cho chúng tôi câu hỏi của bạn và chúng tôi sẽ phản hồi nhanh chóng.",
  },
];

const Services: FC = () => {
  return (
    <div className="grid grid-cols-1 gap-6 md:grid-cols-3 px-4 md:px-10 py-10 bg-gray-50">
      {services.map((service, idx) => (
        <div
          key={idx}
          className="bg-white rounded-2xl shadow-sm hover:shadow-md transition duration-300 p-8 flex flex-col items-center text-center group"
        >
          <div className="text-4xl text-red-500 mb-4 group-hover:scale-110 transition-transform duration-300">
            <FontAwesomeIcon icon={service.icon} />
          </div>
          <h3 className="text-xl font-bold mb-2">{service.title}</h3>
          <p className="text-gray-600 text-base">{service.description}</p>
        </div>
      ))}
    </div>
  );
};

export default Services;
