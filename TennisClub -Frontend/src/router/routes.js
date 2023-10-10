const routes = [
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      { path: "", component: () => import("pages/Index.vue") },
      {
        path: "/ranking",
        component: () => import("pages/app/RankingPage.vue"),
        meta: { requiresAuth: true, roles: ["User"] }
      },
      {
        path: "/head2head",
        component: () => import("pages/app/HeadToHeadPage.vue"),
        meta: { requiresAuth: true, roles: ["User"] }
      },
      {
        path: "/player",
        component: () => import("pages/app/PlayerDetailsPage.vue"),
        meta: { requiresAuth: true, roles: ["User"] }
      },
      {
        path: "/tournament",
        component: () => import("pages/app/TournamentPage.vue"),
        meta: { requiresAuth: true, roles: ["User"] }
      }
    ]
  },
  {
    path: "/administration",
    component: () => import("layouts/AdminLayout.vue"),
    meta: { requiresAuth: true, roles: ["Admin"] },
    children: [
      {
        path: "players",
        component: () => import("pages/admin/PlayersPage.vue")
      },
      {
        path: "tournaments",
        component: () => import("pages/admin/TournamentsPage.vue")
      }
    ]
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: "*",
    component: () => import("pages/Error404.vue")
  }
];

export default routes;
