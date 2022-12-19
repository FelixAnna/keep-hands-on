import 'package:path/path.dart';
import 'package:sqflite/sqflite.dart';

class SQFliteService {
  static Future<Database> getDatabase() async {
    return openDatabase(
      // Set the path to the database. Note: Using the `join` function from the
      // `path` package is best practice to ensure the path is correctly
      // constructed for each platform.
      join(await getDatabasesPath(), 'hss2.db'),
      // When the database is first created, create a table to store dogs.
      onCreate: (db, version) {
        // Run the CREATE TABLE statement on the database.
        db.execute(
          'CREATE TABLE Chats(ownerId TEXT, chatId TEXT, latestMsge TEXT, unreadCount TEXT, time TEXT)',
        );
        db.execute(
          'CREATE TABLE Messages(id INTEGER, chatId TEXT, sender TEXT, target TEXT, content TEXT, msgTime TEXT)',
        );
        return;
      },
      // Set the version. This executes the onCreate function and provides a
      // path to perform database upgrades and downgrades.
      version: 1,
    );
  }
}
