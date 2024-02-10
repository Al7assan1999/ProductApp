import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'ProductApp',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44311/',
    redirectUri: baseUrl,
    clientId: 'ProductApp_App',
    responseType: 'code',
    scope: 'offline_access ProductApp',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44311',
      rootNamespace: 'ProductApp',
    },
  },
} as Environment;
