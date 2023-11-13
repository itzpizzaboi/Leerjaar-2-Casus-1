<script setup>
    import { ref, defineProps } from 'vue';
    import { doUserLogin } from '../helper.js';
    import { createRouter, createWebHashHistory } from 'vue-router';

    const props = defineProps(['redirectUrl']);
    const loginFailed = ref(false);

    const email = ref('');
    const password = ref('');

    async function getInfoAndDoLogin(event) {
        event.preventDefault();

        const success = await doUserLogin(email.value, password.value);

        if (success !== true) {
            loginFailed.value = true;
            return;
        }

        const router = createRouter({
            history: createWebHashHistory(),
            routes: []
        });

        if (props.redirectUrl === undefined) {
            router.push({ path: '/dashboard' });
        } else {
            router.push({ path: props.redirectUrl });
        }
    }
</script>


<template>
    <div class="min-h-screen flex items-center justify-center bg-gray-100">
        <div class="flex w-full max-w-4xl">
            <!-- Floating Text Boxes -->
            <div class="flex-1 p-8 bg-blue-500 text-white rounded-lg shadow-md">
                <div class="mb-4">
                    <h2 class="text-2xl font-semibold mb-2">Lorem ipsum.</h2>
                    <p class="text-gray-200">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque ac condimentum libero, vel rhoncus justo. Donec ullamcorper, justo vitae blandit venenatis, mi ipsum tristique metus, et semper felis eros viverra justo. Aenean id purus vitae mauris pharetra tempor pulvinar id tellus. Donec id porttitor eros. Vestibulum diam ante, malesuada vitae diam sed, scelerisque cursus urna. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Proin mi neque, feugiat vehicula tellus ac, tempor iaculis sem. Quisque nibh mi, aliquam eget ex quis, scelerisque aliquam ex. Cras lacinia congue dignissim. Mauris in dolor ut massa feugiat condimentum sed eget orci</p>
                </div>
            </div>

            <!-- Login Screen -->
            <div class="flex-1 p-8 bg-white rounded-lg shadow-md">
                <h1 class="text-4xl font-extrabold text-gray-800 mb-6">Login</h1>
                <form @submit.prevent="getInfoAndDoLogin">
                    <div class="mb-4">
                        <label for="email" class="block text-sm font-medium text-gray-600">Email</label>
                        <input type="email"
                               id="email"
                               name="email"
                               v-model="email"
                               class="mt-1 p-3 w-full rounded-md border focus:outline-none focus:border-blue-500"
                               placeholder="example@example.com" />
                    </div>
                    <div class="mb-4">
                        <label for="password" class="block text-sm font-medium text-gray-600">Password</label>
                        <input type="password"
                               id="password"
                               name="password"
                               v-model="password"
                               class="mt-1 p-3 w-full rounded-md border focus:outline-none focus:border-blue-500"
                               placeholder="********" />
                    </div>
                    <button type="submit"
                            class="w-full bg-blue-500 text-white p-3 rounded-md hover:bg-blue-600 focus:outline-none focus:shadow-outline-blue">
                        Login
                    </button>
                </form>
            </div>
        </div>
    </div>
</template>

