// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: true,
  APP_TITLE: 'OMS Tasty Kitchen (Testing)',
  APP_CLIENT_URL: 'https://omsclient.kitchen.dvg-lc.com',
  APP_API_URL: 'https://omsapi.kitchen.dvg-lc.com',
  loginSSO: false,
  loginUrl: 'https://sso.daivietgroup.net/account/logincallback',
  CTVPostingGroupRoleId: 9,
  CTVArticleGroupRoleId: 7,
  XMLFaceBookCacheMinute: 5,
  PostingStaffGroupRoleId: 13,
  LeaderBTVArticleGroupRoleId: 6,
  CMSExternalAPIUrl: 'http://localhost:49953/api/xmlfacebook/GenXML?accessToken=AutoIndianAdmin|replacethistexttoapassword'
};
