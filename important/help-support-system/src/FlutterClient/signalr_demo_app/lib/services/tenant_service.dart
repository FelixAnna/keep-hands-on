import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart' as http;
import '../config/env.dart';

class TenantService {
  final IAppEnv env;

  TenantService({required this.env});

  Future<dynamic> loadTenants(status) async {
    var body = json.encode({"tenantStatus": status});
    var start = DateTime.now().millisecondsSinceEpoch;
    final response = await http.post(
      Uri.parse(env.userApiAddress + "/api/tenants/query"),
      headers: {
        HttpHeaders.contentTypeHeader: 'application/json',
      },
      encoding: Encoding.getByName('utf-8'),
      body: body,
    );
    final responseJson = jsonDecode(response.body);
    print(DateTime.now().millisecondsSinceEpoch - start);
    if (response.statusCode == HttpStatus.ok) {
      return responseJson;
    } else {
      print('error: ' + response.toString());
      throw new Exception(response.toString());
    }
  }
}
