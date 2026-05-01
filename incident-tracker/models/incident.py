from datetime import datetime
from utils.classifier import detect_type, detect_severity


# Base Class

class Incident:
    def __init__(self, id, title, description, reported_by, timestamp, assigned_team):
        self.id = id
        self.title = title
        self.description = description
        self.reported_by = reported_by
        self.timestamp = datetime.fromisoformat(timestamp.replace('Z', '+00:00'))
        self.assigned_team = assigned_team

        self._severity = None
        self.type = None
        self.ticket_ids = {}  

    # Must be overridden in subclasses

    def classify(self):
        raise NotImplementedError("Subclasses must implement classify()")

    @property
    def severity(self):
        return self._severity

    def to_dict(self):
        return {
            "id": self.id,
            "title": self.title,
            "description": self.description,
            "type": self.type,
            "severity": self.severity,
            "assigned_team": self.assigned_team,
            "tickets": self.ticket_ids
        }

    def __str__(self):
        return f"{self.id} | {self.type} | {self.severity}"

    def __repr__(self):
        return f"Incident({self.id}, {self.type}, {self.severity})"

    # For sorting incidents by severity
    def __lt__(self, other):
        order = {"critical": 0, "high": 1, "medium": 2, "low": 3}
        return order.get(self.severity, 4) < order.get(other.severity, 4)


# Network Incident

class NetworkIncident(Incident):
    def __init__(self, *args, affected_host=None, protocol=None, **kwargs):
        super().__init__(*args, **kwargs)
        self.affected_host = affected_host
        self.protocol = protocol

    def classify(self):
        text = self.title + " " + self.description
        self.type = "network"
        self._severity = detect_severity(text)

    def escalate(self):
        print(f"🚨 Escalating network issue {self.id} to on-call team")


# App Incident

class AppIncident(Incident):
    def __init__(self, *args, app_name=None, error_code=None, **kwargs):
        super().__init__(*args, **kwargs)
        self.app_name = app_name
        self.error_code = error_code

    def classify(self):
        text = self.title + " " + self.description
        self.type = "app"
        self._severity = detect_severity(text)

    def get_stack_trace(self):
        return f"Stack trace for {self.id}: NullPointerException at line 42"


# Security Incident

class SecurityIncident(Incident):
    def __init__(self, *args, threat_type=None, source_ip=None, **kwargs):
        super().__init__(*args, **kwargs)
        self.threat_type = threat_type
        self.source_ip = source_ip

    def classify(self):
        text = self.title + " " + self.description
        self.type = "security"
        self._severity = detect_severity(text)

    def notify_soc(self):
        print(f"🔐 Alerting SOC team for incident {self.id}")


# Factory Function (IMPORTANT)

def create_incident(data):
    from utils.classifier import detect_type

    text = data["title"] + " " + data["description"]
    incident_type = detect_type(text)

    if incident_type == "network":
        return NetworkIncident(**data)
    elif incident_type == "security":
        return SecurityIncident(**data)
    elif incident_type == "app":
        return AppIncident(**data)
    else:
        return AppIncident(**data)


# Batch Generator

def batch_incidents(incidents, batch_size=3):
    """
    Yield incidents in batches
    """
    for i in range(0, len(incidents), batch_size):
        yield incidents[i:i + batch_size]