import { createRouter, createWebHistory } from 'vue-router'


const routes = [
  {
    path: "/",
    redirect: "/dashboard",
    component: () => import("@/layouts/Layout.vue"),
    children: [
        {
            name: "dashboard",
            alias: "painel",
            path: "/dashboard",
            component: () => import(/* webpackChunName: "dashboard"*/ "@/views/dashboard/Index.vue"),
        },
        {
            name: "clientes",
            alias: "/cliente",
            path: "/clientes",
            component: () => import(/* webpackChunName: "clientes"*/ "@/views/clientes/Index.vue"),
        },
        {
          name: "formulario-clientes",
          path: "/clientes/formulario/:id?",
          component: () => import(/* webpackChunName: "clientes"*/ "@/views/clientes/Formulario.vue"),
        },
        {
          name: "vendas",
          path: "/vendas",
          component: () => import(/* webpackChunName: "vendas"*/ "@/views/vendas/Index.vue"),
        },
        {
          name: "formulario-vendas",
          path: "/vendas/formulario/:id?",
          component: () => import(/* webpackChunName: "vendas"*/ "@/views/vendas/Formulario.vue"),
        },         
    ],
  },

  {
    path: "/",
    redirect: "/login",
    name: "login",
    component: () => import(/* webpackChunName: "LayoutBlank"*/ "@/layouts/LayoutBlank.vue"),
    children: [
        {
            name: "login",
            path: "/login",
            component: () => import(/* webpackChunName: "Login"*/ "@/views/autenticacao/Login.vue"),
        },
    ],
  },  

  {
    path: '/:pathMatch(.*)',
    redirect: "/404",
    name: "404",
    component: () => import(/* webpackChunName: "404"*/ "@/views/404.vue"),
    children: [
      {
          name: "404",
          path: '/:pathMatch(.*)',
          component: () => import(/* webpackChunName: "404"*/ "@/views/404.vue"),
      },
    ]
  }, 
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})


export default router
