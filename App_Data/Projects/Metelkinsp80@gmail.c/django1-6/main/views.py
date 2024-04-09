from django.shortcuts import render

def index(request):
    return render(request, 'main/index.html')
def about(request):
    return render(request, 'main/onas.html')
def kafe(request):
    return render(request, 'main/kafe.html')
def kontakt(request):
    return render(request, 'main/kontakt.html')
def market(request):
    return render(request, 'main/mgazin.html')
def online(request):
    return render(request, 'main/online.html')