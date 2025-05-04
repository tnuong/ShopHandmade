import axios from "axios";

export type ProviceResource = {
    _id: string;
    name: string;
    type: string;
    slug: string;
    name_with_type: string;
    code: string;
    isDeleted: boolean;
}

export type DistrictResource = {
    _id: string,
    name: string,
    type: string,
    slug: string,
    name_with_type: string,
    path: string,
    path_with_type: string,
    code: string,
    parent_code: string,
    isDeleted: false
}

export type WardResource = {
    _id: string,
    name: string,
    type: string,
    slug: string,
    name_with_type: string,
    path: string,
    path_with_type: string,
    code: string,
    parent_code: string,
    isDeleted: false
}


export type AddressOption = {
    label: string;
    value: string;
}

type AddressResponse<T> = {
    data: T[]
}

class AddressService {

    async getProvinces(): Promise<AddressOption[]> {
        const response = await axios.get('https://vn-public-apis.fpo.vn/provinces/getAll?limit=63');
        if (response.status === 200) {
            console.log(response.data)
            const data = response.data.data as AddressResponse<ProviceResource>;
            return data.data.map(province => ({
                label: province.name_with_type,
                value: province.code
            } as AddressOption))
        }

        return []
    }

    async getDistrictsByProvinceId(provinceId: string): Promise<AddressOption[]> {
        const response = await axios.get('https://vn-public-apis.fpo.vn/districts/getByProvince?provinceCode=' + provinceId);
        if (response.status === 200) {
            const data = response.data.data as AddressResponse<DistrictResource>;
            return data.data.map(district => ({
                label: district.name_with_type,
                value: district.code
            } as AddressOption))
        }

        return []
    }

    async getWardsByDistrictId(districtId: string): Promise<AddressOption[]> {
        const response = await axios.get('https://vn-public-apis.fpo.vn/wards/getByDistrict?districtCode=' + districtId);
        if (response.status === 200) {
            const data = response.data.data as AddressResponse<WardResource>;
            return data.data.map(ward => ({
                label: ward.name_with_type,
                value: ward.code
            } as AddressOption))
        }

        return []
    }

}

const addressService = new AddressService();
export default addressService;