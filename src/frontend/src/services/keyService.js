import http from "./httpService";
import configInfo from '../config.json';

const apiEndpoint = configInfo.outline + "/keys";

function keyUrl(id) {
    return `${apiEndpoint}/${id}`;
}
  
export async function getKeys() {
    const response = await http.get(apiEndpoint);
    return response;
}
  
export async function getKey(keyId) {
    const response = await http.get(keyUrl(keyId));
    return response;
}
  
export async function createKey(createKeyDto) {
    const response = await http.post(apiEndpoint, { 
        name: createKeyDto.name,
        countryId: createKeyDto.countryId != "" ? createKeyDto.countryId : null,
    });
    return response;
}

export async function updateKey(keyId, updateKeyDto) {
    const response = await http.put(keyUrl(keyId), { 
        name: updateKeyDto.name,
    });
    return response;
}
  
export async function deleteKey(keyId) {
    const response = await http.delete(keyUrl(keyId));
    return response;
}

export async function transferKeysToNewServer(sourceId, destId) {
    const response = await http.put(apiEndpoint + `/server/${sourceId}`, {serverId: destId != ""? destId : null });
    return response;
}

export async function transferKeyToNewServer(keyId, destId) {
    const response = await http.put(apiEndpoint + `/${keyId}/server`, {serverId: destId != ""? destId : null});
    return response;
}