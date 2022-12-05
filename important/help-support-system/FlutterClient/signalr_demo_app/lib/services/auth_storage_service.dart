import 'dart:convert';

import '../utils/localstorage_service.dart';

class AuthStorageService {
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

  ClearLoginResult() async => {
        await LocalStorageService.save(LocalStorageService.JWT_KEY, ''),
        await LocalStorageService.save(LocalStorageService.PROFILE, ''),
      };

  Future<String> GetUserName() {
    return LocalStorageService.get(LocalStorageService.REMEMBER_USERNAME);
  }
}
