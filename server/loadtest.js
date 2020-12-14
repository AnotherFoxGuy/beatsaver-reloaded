import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
  vus: 50,
  duration: '30s',
};
export default function () {
  http.get('https://localhost/api/users/find/5fd779695e307f3d62e4eff8');
  //http.get('https://localhost/api/users/find/5fd75b8f7ec3a40f14f9bd43');
}
