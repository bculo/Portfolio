import http from 'k6/http';
import { group, check, sleep } from 'k6';

const REALM_ID = 'PortfolioRealm';
const CLIENT_ID = 'k6-client';
const CLIENT_SECRET = '1a2u2rRc25Gf6GJThGMUBktDt106ncnP';
const API_URL = "http://localhost:5276";
const AUTHORIZATION_URL = "http://localhost:8080";

export function getJwtToken() {
  const url = `${AUTHORIZATION_URL}/realms/${REALM_ID}/protocol/openid-connect/token`;

  const requestBody = {
    grant_type: 'client_credentials',
    client_id: CLIENT_ID,
    client_secret: CLIENT_SECRET,
  };

  const response = http.post(url, requestBody);
  const responseJson = response.json();
  console.log(responseJson);
  return `Bearer ${responseJson["access_token"]}`;
}


export function setup() {
  return {
    'token': getJwtToken()
  };
}

export const options = {
  stages: [
    { duration: '25s', target: 50 }, // traffic ramp-up from 1 to 50 users over 25s.
    { duration: '25s', target: 50 }, // stay at 100 users for 25 seconds
    { duration: '10s', target: 0 }, // ramp-down to 0 users
  ],
};


export default function (data) {
  const url = API_URL + `/api/v1/News/GetLatestNews`;

  const params = {
    headers: {
      'Content-Type': 'application/json',
      'Authorization': data["token"]
    },
  };

  const res = http.get(url, params);

  check(res, {
    'Ok': (r) => res.status === 200,
    'Unauthorized': (r) => res.status === 401,
    'Bad request': (r) => res.status === 400,
  });

  sleep(1)
}