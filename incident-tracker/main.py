import json

from models.incident import create_incident, batch_incidents
from models.report import ReportGenerator

from services.jira import create_ticket as create_jira_ticket
from services.servicenow import create_ticket as create_snow_ticket
from services.azure_boards import create_ticket as create_azure_ticket

from utils.helpers import get_critical_incidents, count_by_team


def load_incidents(file_path="data/incidents.json"):
    with open(file_path, "r") as f:
        return json.load(f)


def main():
    print("Starting Incident Auto-Triage System...\n")

    # Load data
    raw_data = load_incidents()

    incidents = []

    for item in raw_data:
        inc = create_incident(item)
        inc.classify()
        incidents.append(inc)

    print(f"Loaded and classified {len(incidents)} incidents\n")

    incidents.sort()

    for batch in batch_incidents(incidents, batch_size=3):
        print("Processing new batch...\n")

        for inc in batch:
            print(f"Processing {inc.id} ({inc.type} | {inc.severity})")

            ticket_ids = {}

            # Jira
            try:
                ticket_ids["jira"] = create_jira_ticket(inc)
            except Exception as e:
                print(f"Jira failed: {e}")
                ticket_ids["jira"] = "FAILED"

            # ServiceNow
            try:
                ticket_ids["snow"] = create_snow_ticket(inc)
            except Exception as e:
                print(f"ServiceNow failed: {e}")
                ticket_ids["snow"] = "FAILED"

            # Azure Boards
            try:
                ticket_ids["azure"] = create_azure_ticket(inc)
            except Exception as e:
                print(f"Azure failed: {e}")
                ticket_ids["azure"] = "FAILED"

            # Store ticket IDs in incident
            inc.ticket_ids = ticket_ids

        print("Batch completed\n")

    critical_incidents = get_critical_incidents(incidents)
    team_counts = count_by_team(incidents)

    print("Summary:")
    print(f"Critical Incidents: {len(critical_incidents)}")
    print(f"Incidents by Team: {team_counts}\n")

    report = ReportGenerator(incidents)
    report.generate_html()
    report.export_json()

    print("Process completed successfully!")


# Entry Point
if __name__ == "__main__":
    main()