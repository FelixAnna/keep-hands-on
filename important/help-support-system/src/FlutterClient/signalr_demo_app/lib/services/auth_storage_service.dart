import 'dart:convert';

import '../utils/localstorage_service.dart';

class AuthStorageService {
  /// Save login info to storage: token, profile, userName(if [keepUserName] is true)
  SaveLoginResult(response, keepUserName) async => {
        await LocalStorageService.save(
            LocalStorageService.JWT_KEY, response["accessToken"]),
        await LocalStorageService.save(
            LocalStorageService.PROFILE, jsonEncode(response["profile"])),
        if (keepUserName)
          {
            await LocalStorageService.save(
                LocalStorageService.REMEMBER_USERNAME,
                response["profile"]["email"])
          }
        else
          {
            await LocalStorageService.save(
                LocalStorageService.REMEMBER_USERNAME, '')
          }
      };

  /// Get last login cache: token, profile, saved userName from storage
  GetLoginCache() async {
    var token = await LocalStorageService.get(LocalStorageService.JWT_KEY);
    var profile = await LocalStorageService.get(LocalStorageService.PROFILE);
    var userName =
        await LocalStorageService.get(LocalStorageService.REMEMBER_USERNAME);

    return [token, profile, userName];
  }

  /// Clear login info from storage: token, profile
  ClearLoginResult() async => {
        await LocalStorageService.save(LocalStorageService.JWT_KEY, ''),
        await LocalStorageService.save(LocalStorageService.PROFILE, ''),
      };

  SaveTenantsResult(response) async => {
        await LocalStorageService.save(
            LocalStorageService.TENANTS_KEY, jsonEncode(response)),
      };

  GetTenantsCache() async {
    var tenants =
        await LocalStorageService.get(LocalStorageService.TENANTS_KEY);
    return tenants;
  }

  ClearTenantsResult() async => {
        await LocalStorageService.save(LocalStorageService.TENANTS_KEY, ''),
      };
}
