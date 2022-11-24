import "package:flutter/services.dart" as s;
import 'package:yaml/yaml.dart';

Future<String> loadConfig(section) async {
  final yamlString = await s.rootBundle.loadString('assets/config.yaml');
  final dynamic yamlMap = loadYaml(yamlString);

  return yamlMap[section];
}