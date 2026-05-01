import uuid
import logging
import base64
import requests

from utils.decorators import log_call, retry
from config import MOCK_API, JIRA_CONFIG


# Convert text → ADF format
def convert_to_adf(text):
    return {
        "type": "doc",
        "version": 1,
        "content": [
            {
                "type": "paragraph",
                "content": [
                    {
                        "type": "text",
                        "text": text
                    }
                ]
            }
        ]
    }


# Create Jira Ticket

@log_call
@retry(times=3, delay=2)
def create_ticket(incident):

    payload = {
        "fields": {
            "summary": incident.title,
            "description": convert_to_adf(incident.description), 
            "issuetype": {"name": "Task"},  
            "priority": {"name": map_severity_to_priority(incident.severity)},
            "project": {"key": JIRA_CONFIG["project_key"]},
            "labels": [incident.type]
        }
    }

    if MOCK_API:
        ticket_key = f"MOCK-JIRA-{uuid.uuid4().hex[:5].upper()}"
        print(f"[MOCK] Jira Ticket Created: {ticket_key}")
        return ticket_key

    url = f"{JIRA_CONFIG['base_url']}rest/api/3/issue"

    token = base64.b64encode(
        f"{JIRA_CONFIG['email']}:{JIRA_CONFIG['api_token']}".encode()
    ).decode()

    headers = {
        "Authorization": f"Basic {token}",
        "Content-Type": "application/json"
    }

    response = requests.post(url, json=payload, headers=headers)

    print("Jira Response:", response.status_code, response.text)

    if response.status_code == 201:
        return response.json()["key"]
    else:
        raise Exception(f"Jira API Error: {response.text}")


# Helper
def map_severity_to_priority(severity):
    return {
        "critical": "Highest",
        "high": "High",
        "medium": "Medium",
        "low": "Low"
    }.get(severity, "Low")