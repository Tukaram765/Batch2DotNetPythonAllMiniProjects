import uuid
import logging

from utils.decorators import log_call, retry
from config import MOCK_API


# Create Azure Boards Work Item

@log_call
@retry(times=3, delay=2)
def create_ticket(incident):
    """
    Creates an Azure Boards work item (Bug)

    Returns:
        ticket_id (int or str)
    """

    # JSON Patch format (list of operations)
    payload = [
        {
            "op": "add",
            "path": "/fields/System.Title",
            "value": incident.title
        },
        {
            "op": "add",
            "path": "/fields/System.Description",
            "value": incident.description
        },
        {
            "op": "add",
            "path": "/fields/Microsoft.VSTS.Common.Priority",
            "value": map_severity_to_priority(incident.severity)
        },
        {
            "op": "add",
            "path": "/fields/System.AssignedTo",
            "value": incident.assigned_team
        }
    ]

    if MOCK_API:
        # Mock mode
        ticket_id = f"MOCK-AZURE-{uuid.uuid4().hex[:6].upper()}"
        print(f"[MOCK] Azure Work Item Created: {ticket_id}")
        print(f"[MOCK] Payload: {payload}\n")
        return ticket_id

    else:
        import requests
        import base64

        org = "your-org"
        project = "your-project"
        pat = "your-personal-access-token"

        url = f"https://dev.azure.com/{org}/{project}/_apis/wit/workitems/$Bug?api-version=7.1"

        token = base64.b64encode(f":{pat}".encode()).decode()

        headers = {
            "Authorization": f"Basic {token}",
            "Content-Type": "application/json-patch+json"
        }

        response = requests.post(url, json=payload, headers=headers)

        if response.status_code == 200 or response.status_code == 201:
            data = response.json()
            ticket_id = data["id"]
            logging.info(f"Azure Work Item Created: {ticket_id}")
            return ticket_id
        else:
            raise Exception(f"Azure API Error: {response.text}")


# Update Work Item

@log_call
@retry(times=3, delay=2)
def update_ticket(ticket_id, priority):
    """
    Updates Azure work item priority
    """

    if MOCK_API:
        print(f"[MOCK] Updating Azure Work Item {ticket_id} → Priority: {priority}")
        return True

    else:
        import requests
        import base64

        org = "your-org"
        project = "your-project"
        pat = "your-personal-access-token"

        url = f"https://dev.azure.com/{org}/{project}/_apis/wit/workitems/{ticket_id}?api-version=7.1"

        token = base64.b64encode(f":{pat}".encode()).decode()

        headers = {
            "Authorization": f"Basic {token}",
            "Content-Type": "application/json-patch+json"
        }

        payload = [
            {
                "op": "add",
                "path": "/fields/Microsoft.VSTS.Common.Priority",
                "value": priority
            }
        ]

        response = requests.patch(url, json=payload, headers=headers)

        if response.status_code == 200:
            logging.info(f"Azure Work Item {ticket_id} updated successfully")
            return True
        else:
            raise Exception(f"Azure Update Error: {response.text}")


# Helper Function

def map_severity_to_priority(severity):
    """
    Maps severity → Azure priority (1 = highest)
    """
    mapping = {
        "critical": 1,
        "high": 2,
        "medium": 3,
        "low": 4
    }
    return mapping.get(severity, 4)