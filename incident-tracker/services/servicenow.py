import uuid
import logging
import requests

from utils.decorators import log_call, retry
from config import MOCK_API, SERVICENOW_CONFIG

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Create ServiceNow Ticket

@log_call
@retry(times=3, delay=2)
def create_ticket(incident):

    payload = {
        "short_description": incident.title,
        "description": incident.description,
        "urgency": map_severity_to_urgency(incident.severity),
        "category": incident.type
    }

    if MOCK_API:
        ticket_id = f"MOCK-SNOW-{uuid.uuid4().hex[:6].upper()}"
        print(f"[MOCK] ServiceNow Ticket Created: {ticket_id}")
        return ticket_id

    url = f"{SERVICENOW_CONFIG['instance_url']}/api/now/table/incident"

    response = requests.post(
        url,
        auth=(SERVICENOW_CONFIG["username"], SERVICENOW_CONFIG["password"]),
        headers={"Content-Type": "application/json"},
        json=payload
    )

    print("ServiceNow Response:", response.status_code, response.text)

    if response.status_code == 201:
        return response.json()["result"]["number"]  
    else:
        raise Exception(f"ServiceNow API Error: {response.text}")


# Helper
def map_severity_to_urgency(severity):
    return {
        "critical": 1,
        "high": 1,
        "medium": 2,
        "low": 3
    }.get(severity, 3)