import { createRouter, createWebHistory, createWebHashHistory } from 'vue-router';
import { createApp } from 'vue';
import App from './App.vue'
import HelloWorld from './components/HelloWorld.vue';  
import Homepage from './components/Homepage.vue'

const routes = [
    { path: '/', component: Homepage },
    { path: '/HelloWorld', component: HelloWorld },
    
]

const router = createRouter({
    history: createWebHashHistory(),
    routes,
})

const app = createApp(App);

app.use(router);

app.mount('#app');
