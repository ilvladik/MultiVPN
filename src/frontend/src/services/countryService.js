import http from "./httpService";
import configInfo from '../config.json';

const apiEndpoint = configInfo.outline + "/countries";

function countryUrl(id) {
    return `${apiEndpoint}/${id}`;
}
  
export async function getCountries() {
    const response = await http.get(apiEndpoint);
    return response;
}

export async function getOnlyUsedCountries() {
    const response = await http.get(apiEndpoint + "?onlyUsed=true");
    return response;
}
  
export async function getCountry(countryId) {
    const response = await http.get(countryUrl(countryId));
    return response;
}
  
export async function createCountry(createCountryDto) {
    const response = await http.post(apiEndpoint, { 
        name: createCountryDto.name, 
    });
    return response;
}
  
export async function deleteCountry(countryId) {
    const response = await http.delete(countryUrl(countryId));
    return response;
}