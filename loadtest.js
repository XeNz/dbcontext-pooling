import http from 'k6/http';
import {check, sleep} from 'k6';
import {randomIntBetween} from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';

export const options = {
    stages: [
        {duration: '20s', target: 50},
        {duration: '40s', target: 300},
        {duration: '20s', target: 50},
        {duration: '30s', target: 0},
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'], // 95% of requests must complete below 500ms
    },
};

export default function () {
    const baseUrl = 'http://localhost:5000';
    const campaignId = randomIntBetween(1, 300);

    const addResponse = http.post(`${baseUrl}/api/test?campaignId=${campaignId}`);

    check(addResponse, {
        'Add status is 200': (r) => r.status === 200,
        'Add response has id': (r) => JSON.parse(r.body).id !== undefined,
    });

    sleep(1);

    const countResponse = http.get(`${baseUrl}/api/test?campaignId=${campaignId}`);

    check(countResponse, {
        'Count status is 200': (r) => r.status === 200,
        'Count response has count': (r) => JSON.parse(r.body).count !== undefined,
    });

    sleep(1);
}
