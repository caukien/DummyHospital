import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/book-store',
        name: '::Menu:BookStore',
        iconClass: 'fas fa-book',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/books',
        name: '::Menu:Books',
        parentName: '::Menu:BookStore',
        layout: eLayoutType.application,
      },
      {
        path: '/Area-Management',
        name: 'Area',
        iconClass: 'fas fa-city',
        order: 3,
        layout: eLayoutType.application,
      },
      {
        path: '/provinces',
        name: 'Province',
        parentName: 'Area',
        layout: eLayoutType.application,
      },
      {
        path: '/districts',
        name: 'Districts',
        parentName: 'Area',
        layout: eLayoutType.application,
      },
      {
        path: '/communes',
        name: 'Commune',
        parentName: 'Area',
        layout: eLayoutType.application,
      },
      {
        path: '/Care',
        name: 'Care',
        iconClass: 'fas fa-city',
        order: 4,
        layout: eLayoutType.application,
      },
      {
        path: '/hospitals',
        name: 'Hospital',
        parentName: 'Care',
        layout: eLayoutType.application,
      },
      {
        path: '/patients',
        name: 'Patient',
        parentName: 'Care',
        layout: eLayoutType.application,
      },
    ]);
  };
}
