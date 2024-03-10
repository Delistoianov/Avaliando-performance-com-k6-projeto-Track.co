import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    stages: [
        { duration: '1m', target: 20 }, // 20 usuários em 1 min
        { duration: '3m', target: 20 }, // 20 usuários em 3 min
        { duration: '1m', target: 0 }, // 0 usuários em 1 min
    ],
};

export default function () {
    const url = 'http://localhost:5244/widgets'; 

    let params = {
        headers: {
            'Content-Type': 'application/json',
        },
        insecureSkipTLSVerify: true,
    };

    //get all widgets
    let getWidgetsResponse = http.get(url);
    check(getWidgetsResponse, { 'status was 200': (r) => r.status === 200 });
    sleep(1);

    //get widget by id
    let getWidgetResponse = http.get(`${url}/1`); 
    check(getWidgetResponse, { 'status was 200': (r) => r.status === 200 });
    sleep(1);

    //post widget
    let payload = JSON.stringify({
        title: 'title',
        link: 'link',
        question: 'question',
    });
    

    let postWidgetResponse = http.post(url, payload, params); 
    check(postWidgetResponse, { 'status was 201': (r) => r.status === 201 });
    sleep(1);

    //post nps
    let npsPayload = JSON.stringify({
        widgetId: 'widgetId',
        answer: 'answer',
        rating: 'rating'
    });
    
    let postNpsResponse = http.post(`${url}/1/nps`, npsPayload, params); 
    check(postNpsResponse, { 'status was 201': (r) => r.status === 201 });
    sleep(1);
    
}
