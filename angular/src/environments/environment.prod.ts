import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'ecommerce',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44372/',
    redirectUri: baseUrl,
    clientId: 'ecommerce_App',
    responseType: 'code',
    scope: 'offline_access ecommerce',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44374',
      rootNamespace: 'ecommerce',
    },
  },
} as Environment;
