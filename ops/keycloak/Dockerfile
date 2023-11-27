FROM quay.io/keycloak/keycloak:23.0 as builder

ENV KC_HEALTH_ENABLED=true
ENV KC_METRICS_ENABLED=true
ENV KC_DB=mssql
ENV KC_TRANSACTION_XA_ENABLED=false

WORKDIR /opt/keycloak
RUN /opt/keycloak/bin/kc.sh build --db=mssql --transaction-xa-enabled=false

FROM quay.io/keycloak/keycloak:23.0
COPY --from=builder /opt/keycloak/ /opt/keycloak/

ENTRYPOINT ["/opt/keycloak/bin/kc.sh"]