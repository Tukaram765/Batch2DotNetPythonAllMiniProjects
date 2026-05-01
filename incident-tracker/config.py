import os


# Global Flag
MOCK_API = True


# ServiceNow Configuration
SERVICENOW_CONFIG = {
    "instance_url": "https://dev392397.service-now.com",
    "username": "admin",
    "password": "gj$6HpSC9+Hm"
}


# Jira Configuration
JIRA_CONFIG = {
    "base_url": "https://tukaramincidenttracker.atlassian.net/",
    "email": "tukarambhangre197@gmail.com",
    "api_token": os.getenv("API_TOKEN"),
    "project_key": "KAN"
}


# Azure Boards Configuration
AZURE_CONFIG = {
    "organization": "your-org",
    "project": "your-project",
    "pat": "your-personal-access-token"
}


# App Settings
APP_CONFIG = {
    "batch_size": 3,
    "log_level": "INFO",
    "default_output_html": "output/report.html",
    "default_output_json": "output/report.json"
}