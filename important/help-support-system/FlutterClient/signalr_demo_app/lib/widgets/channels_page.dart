import 'package:flutter/material.dart';

class ChannelsPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(32),
        child: Row(
          children: [
            const Text('This is channels page'),
          ],
        ),
      ),
    );
  }
}
