import { ReportQuery } from '../areas/admin/pages/Dashboard';
import axiosConfig from '../configuration/axiosConfig';
import { DataResponse, OrderReport, OrderReportByMonth, ProductReport, ReportResource } from "../resources";

class ReportService {
    getReportData(queryParams: ReportQuery) : Promise<DataResponse<ReportResource>> {
        const queryString = new URLSearchParams(queryParams as any).toString();
        console.log(queryString)
        return axiosConfig.get("/api/BaoCaoThongKe?" + queryString);
    }

    getOrderPercentInRangeYear(year: number | string) : Promise<DataResponse<OrderReport[]>> {
        return axiosConfig.get("/api/BaoCaoThongKe/don-hang/nam?nam=" + year);
    }

    getOrderByMonth(month?: Date) : Promise<DataResponse<OrderReportByMonth[]>> {
        return axiosConfig.get("/api/BaoCaoThongKe/don-hang/thang?thang=" + month?.toISOString());
    }

    getTopFiveBestSellerProducts(fromTime?: Date, toTime?: Date) : Promise<DataResponse<ProductReport[]>> {
        return axiosConfig.get('/api/BaoCaoThongKe/top-san-pham?batDau=' + fromTime + "&ketThuc=" + toTime);
    }

}

const reportService = new ReportService();
export default reportService;