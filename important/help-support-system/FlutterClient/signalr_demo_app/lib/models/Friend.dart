class Friend{
  String UserId;
  String Name;
  String Email;
  Friend({required this.UserId,required this.Name,required this.Email});

  Friend.fromJson(Map<String, dynamic> json)
      : UserId = json['userId'],
        Name = json['name'],
        Email = json['email'];

  Map<String, dynamic> toJson() => {
    'userId': this.UserId,
    'name': this.Name,
    'email': this.Email
  };
}