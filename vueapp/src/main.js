import './styles/tailwind.css';
import { createRouter, createWebHistory, createWebHashHistory } from 'vue-router';
import { createApp } from 'vue';
import App from './App.vue'
import Homepage from './components/HomePage.vue'
import Dashboard from './components/Dashboard.vue'; 


const routes = [
    { path: '/', component : Homepage },
    { path: '/dashboard', name: 'dashboard', component: Dashboard}
]

const router = createRouter({
    history: createWebHashHistory(),
    routes,
})

const app = createApp(App);

app.use(router);

app.mount('#app');

