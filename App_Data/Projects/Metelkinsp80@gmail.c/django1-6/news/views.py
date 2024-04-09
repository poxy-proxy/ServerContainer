from django.shortcuts import render
from .forms import AuthUserForm, AuthenticationForm, RegisterUserForm
from .models import Articles
from django.contrib.auth.views import LoginView
from django.contrib.auth.models import User
from django.views.generic import CreateView
from django.urls import reverse_lazy, reverse
from django.contrib import messages
def news_home(request):
    news = Articles.objects.all()
    return render(request, 'main/online.html', {'news': news})
def about(request):
    news = Articles.objects.all()
    return render(request, 'main/onas.html')
def kafe(request):
    news = Articles.objects.all()
    return render(request, 'main/kafe.html')
def kontakt(request):
    news = Articles.objects.all()
    return render(request, 'main/kontakt.html')
def market(request):
    news = Articles.objects.all()
    return render(request, 'main/mgazin.html')
def index(request):
    news = Articles.objects.all()
    return render(request, 'main/index.html')
def login(request):
    news = Articles.objects.all()
    return render(request, 'main/login.html')

class MyprojectLoginView(LoginView):
    template_name = 'main/login.html'
    form_class = AuthUserForm
    success_url = reverse_lazy('news_home')
    def get_success_url(self):
        return self.success_url
class RegisterUserView(CreateView):
    model = User
    template_name = 'main/reg.html'
    success_url = reverse_lazy('news_home')
    success_msg = 'Вы успешно зарегестрировались'
    form_class = RegisterUserForm