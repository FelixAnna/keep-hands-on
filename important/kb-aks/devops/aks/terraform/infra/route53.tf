# Configure DNS record in aws
data "aws_route53_zone" "selected" {
  name         = "metadlw.com"
}

resource "aws_route53_record" "api" {
  zone_id = data.aws_route53_zone.selected.zone_id
  name    = var.backendDNS
  type    = "A"
  ttl     = 300
  records = [azurerm_public_ip.gwIp.ip_address]
}
