output "asumistuki_api_url" {
  description = "Asumistuki API:n julkinen URL"
  value       = "https://${azurerm_container_app.asumistuki_api.ingress[0].fqdn}"
}

output "asumistuki_testipenkki_url" {
  description = "Asumistuki Testipenkin julkinen URL"
  value       = "https://${azurerm_container_app.asumistuki_testipenkki.ingress[0].fqdn}"
}

output "acr_login_server" {
  description = "Container Registry -osoite"
  value       = azurerm_container_registry.main.login_server
}
