import http from "./httpService";
import configInfo from '../config.json';

const apiEndpoint = configInfo.outline + "/servers";

function serverUrl(id) {
    return `${apiEndpoint}/${id}`;
}
  
export async function getServers() {
    const response = await http.get(apiEndpoint);
    return response;
}
  
export async function getServer(serverId) {
    const response = await http.get(serverUrl(serverId));
    return response;
}
  
export async function createServer(createServerDto) {
    const response = await http.post(apiEndpoint, { 
        name: createServerDto.name, 
        apiUrl: createServerDto.apiUrl, 
        countryId: createServerDto.countryId != "" ? createServerDto.countryId : null
    });
    return response;
}

export async function updateServer(updateServerDto, serverId) {
    const response = await http.put(serverUrl(serverId), { 
        name: updateServerDto.name, 
        isAvailable: updateServerDto.isAvailable, 
    });
    return response;
}
  
export async function deleteServer(serverId) {
    const response = await http.delete(serverUrl(serverId));
    return response.data;
}